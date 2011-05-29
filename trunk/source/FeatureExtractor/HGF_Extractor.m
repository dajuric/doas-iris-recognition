classdef HGF_Extractor
    
    properties(Constant, Access = 'public')
        GaborOrientations = [0, pi/4,  pi/2,  3*pi/4];
        PossibleGaborFrequency = [0.025, 0.05, 0.075, 0.1, 0.15, 0.2, 0.4, 0.6, 0.8, 1];
        
        %Scales = [0.5, 1.1, 6]
        %NumberOfPointsPerLayer = [49, 9, 1]
        
        %Scales = [4.51, 9.02, 18.04]
        Scales = [2]
        NumberOfPointsPerLayer = [49]
    end
    
    properties(Access = 'private')
        ImageArray,
        Classes,
        ImageSize
    end
    
    methods
        function this = HGF_Extractor(imageArray, classes)
            this.ImageArray = imageArray;
            this.Classes = classes;
            
            this.ImageSize = sqrt( prod(size(imageArray, 2)) ); %slika je kvadratna
        end
        
        function [featureArray, optimalFrequencies] = GetOptimalFeatures(this)
            
            featureArray = [];  optimalFrequencies = [];
                        
            for scaleIdx = 1 : 1 : size(FeatureExtractor.Scales, 2)
               scale = FeatureExtractor.Scales(scaleIdx);
               numOfPoints = FeatureExtractor.NumberOfPointsPerLayer(scaleIdx);
                
               points = FeatureExtractor.GetPoints(this.ImageSize, numOfPoints);                   
               [featureArrayForLayer, optimalFrequenciesForLayer] = this.ComputeOptimalResponsesForLayer(points, scale);
               
               featureArray = cat(2, featureArray, featureArrayForLayer);
               optimalFrequencies(scaleIdx, 1:size(optimalFrequenciesForLayer, 2)) = optimalFrequenciesForLayer; %frekvencije za pojedini sloj su predane u retke
            end           
           
           for featureVectorIdx = 1 : 1 : size(featureArray, 1) %za broj slika
               featureVector = featureArray(featureVectorIdx, :);
               
               %featureVector = UtilityFunctions.RemoveDC(featureVector);
               %featureVector = UtilityFunctions.ScaleValues(featureVector, -1, 1);
                         
               featureArray(featureVectorIdx, :) = featureVector;
           end         
        end
        
        function featureArray = GetFeatures(this, optimalFrequencies)
            
           featureArray = [];
            
           for scaleIdx = 1 : 1 : size(FeatureExtractor.Scales, 2)
               scale = FeatureExtractor.Scales(scaleIdx);
               numOfPoints = FeatureExtractor.NumberOfPointsPerLayer(scaleIdx);
                
               points = FeatureExtractor.GetPoints(this.ImageSize, numOfPoints);
               optimalFrequenciesForlayer = optimalFrequencies(scaleIdx, :); %frekvencije za pojedini sloj su predane u retke (prvo se koristi samo 49, zatim 9, na kraju 1)
               featureArrayForLayer = this.ComputeResponsesForLayer(points, scale, optimalFrequenciesForlayer);
               
               featureArray = cat(2, featureArray, featureArrayForLayer);            
           end          
                   
           for featureVectorIdx = 1 : 1 : size(featureArray, 1)
               featureVector = featureArray(featureVectorIdx, :);
               
               %featureVector = UtilityFunctions.RemoveDC(featureVector);
               %featureVector = UtilityFunctions.ScaleValues(featureVector, -1, 1);
                         
               featureArray(featureVectorIdx, :) = featureVector;
           end         
        end
        
    end
    
    methods(Access = 'private')
        
        %responses  - niz vektora znaèajki -> [brojSlika x vektorZnaèajki]
        %points     - niz [x y]
        %scale      - skalar (sigma za Gabor)
        %freqencies - frekevencije za pojedini filtar poredane: 
                      %frekv. za sve toèke za orijentaciju 1.. za sve toèke za orijentaciju 2...
        function responses = ComputeResponsesForLayer(this, points, scale, frequencies)
            responses = [];
            
            for idxOrientation = 1 : 1 : length(FeatureExtractor.GaborOrientations)
                orientation = FeatureExtractor.GaborOrientations(idxOrientation);
                              
                for idxPoint=1 : 1 : size(points, 1)
                    point = points(idxPoint, :);   
                    
                    idxFreq = (idxOrientation-1)*length(points) + idxPoint;
                    f = frequencies(idxFreq);
                    
                    responsesForPoint = FeatureExtractor.ComputeResponsesForPosition(this.ImageArray, scale, orientation, f, point);

                    responses = cat(2, responses, responsesForPoint); %nadogradi do potpunog featureVector-a
                end
            end
        end
        
         %responses - niz vektora znaèajki -> [brojSlika vektorZnaèajki] 
           %(vektorZnacajki -> odziv na tocku1, odziv na tocku2...)      
        function [featureArray, optimalFrequencies] = ComputeOptimalResponsesForLayer(this, points, scale)
            
            fprintf('\nScale: %d ->', scale);
            
            featureArray = [];
            for idxOrientation = 1 : 1 : length(FeatureExtractor.GaborOrientations)
                orientation = FeatureExtractor.GaborOrientations(idxOrientation);
                   
                fprintf('   \n Orientation: %d / %d ->\n', idxOrientation, length(FeatureExtractor.GaborOrientations));
                
                for idxPoint=1 : 1 : size(points, 1)
                    point = points(idxPoint, :);      
                    
                    idxInArray = (idxOrientation-1)*length(points) + idxPoint;
                    
                    [optimalFrequency, bestResponse] = this.ChooseOptimalFrequency(scale, orientation, point);                      
                    optimalFrequencies(idxInArray) = optimalFrequency;
                    featureArray = cat(2, featureArray, bestResponse);
                    fprintf('     Toèka: %d / %d. Najbolja frekvencija je %f\n', idxPoint, length(points), optimalFrequency);
                end
            end
        end
               
          %za svaku frekvenciju izraèunaj LDA -> odaberi onu koja maksimizira LDA
        function [optimalFrequency, bestResponse] = ChooseOptimalFrequency(this, scale, orientation, point)
            
            ldaMesaures = []; responsesForPoint = [];
            for frequencyIdx = 1 : 1 : length(FeatureExtractor.PossibleGaborFrequency)
                frequency = FeatureExtractor.PossibleGaborFrequency(frequencyIdx);
                
                %odzivi na sve slike po frekvencijama su stupci
                responsesForPoint(:, frequencyIdx) = FeatureExtractor.ComputeResponsesForPosition(this.ImageArray, scale, orientation, frequency, point);
                                
                lda = LDA(responsesForPoint(:, frequencyIdx), this.Classes);
                lda.Compute();
                ldaMesaures(frequencyIdx) = lda.CalculateFLDMeasure(1);
            end
                 
            maxMesaure = max(ldaMesaures);
            optimalFrequencyIdx = find(ldaMesaures == maxMesaure);   
            optimalFrequencyIdx =  optimalFrequencyIdx(1);
            
            optimalFrequency = FeatureExtractor.PossibleGaborFrequency(optimalFrequencyIdx);
            bestResponse = responsesForPoint(:, optimalFrequencyIdx);
        end
        
    end
    
    methods(Static, Access = 'public')
        
        %raèuna uniformno rasporeðene toèke (jedna toèka je na sredini slike)
        %points         - stupac vektor sa koordinatama toèaka [x y]
        %imageSize      - velièina slike u jednoj dimenziji (slika je pravokutna)
        %numberOfPoints - traženi broj toèaka        
        function points = GetPoints(imageSize, numberOfPoints)
                        
            numberOfPointsPerDimension = round( sqrt(numberOfPoints) ); %gledamo samo za redak/stupac
            
            if numberOfPointsPerDimension ~= sqrt(numberOfPoints)
                error('Broj toèaka po dimenziji slike mora biti jednak stoga broj toèaka mora se moæi korjenovai na cijeli broj');
            end
                
            points = []; 

            numOfSectionsPerDim = numberOfPointsPerDimension + 1;
            pointDistance = imageSize / numOfSectionsPerDim;
            
            pointCords = [];
            pointCord = 0;
            while numberOfPointsPerDimension > 0
                pointCord = pointCord + pointDistance; 
                pointCords = cat(2, pointCords, pointCord);
               
                numberOfPointsPerDimension = numberOfPointsPerDimension -1;
            end
                     
            %uzmi za svaki red jednu koordinatu i i naèini kombinacije
            %(želimo koorsinate (red, stupac)
            for pointIndxX = 1 : 1 : length(pointCords)
                pointCordX = pointCords(pointIndxX);            
                
                for pointIndxY = 1 : 1 : length(pointCords)
                    pointCordY = pointCords(pointIndxY);      
                    point = [pointCordX pointCordY];
                    
                    points = cat(1, points, point);              
                end             
            end   
            
        end 
          
        %za svaku sliku izraèunaj odziv na toèku
        function responses = ComputeResponsesForPosition(imageArray, scale, orientation, frequency, position)
            numOfImages = size(imageArray, 1);         
            imageSize = sqrt( prod(size(imageArray, 2)) );            
            
            gabor = GaborKernel(imageSize, scale, orientation, frequency, position);
            imagParts = gabor.GetImagParts(); 
            imagParts = reshape(imagParts, 1, prod(size(imagParts)) ); %strpaj sve u jednu liniju
            
            responses = [];
            for imageIdx = 1 : 1: size(imageArray, 1)
                
                image = imageArray(imageIdx, :);                                      
                
                filterResponse = imagParts.*image;
                filterResponse = UtilityFunctions.ScaleValues(filterResponse, -1, 1);
                filterResponse = sum(filterResponse.^2); %ovo je DIO featureVectora (toèka)
                            
                responses = cat(1, responses, filterResponse); %vektor stupac su odzivi svi slika na zadane parametre [brSlika x 1]
            end
                    
        end
        
    end   
end