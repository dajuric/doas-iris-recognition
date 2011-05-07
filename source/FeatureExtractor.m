classdef FeatureExtractor
    
    properties(Constant, Access = 'private')
        GaborOrientations = [0, pi/4,  pi/2,  3*pi/4];
        PossibleGaborFrequency = [0.025, 0.05, 0.075, 0.1, 0.15, 0.2, 0.4, 0.6, 0.8, 1];
    end
    
    properties(Access = 'private')
        ImageArray,
        Classes,
        ImageSize
    end
    
    methods
        function this = FeatureExtractor(imageArray, classes)
            this.ImageArray = imageArray;
            this.Classes = classes;
            
            this.ImageSize = sqrt( prod(size(imageArray, 2)) ); %slika je pravokutna
        end
        
        function featureArray = GetFeatures(this)
           points = FeatureExtractor.GetPoints(this.ImageSize, 49);
           featureArray = FeatureExtractor.ComputeResponses(this.ImageArray, points, 2, 0.2);
           
           %OVO NE RADITI
           %featureArray = UtilityFunctions.ScaleValues(featureArray, -10, 10);
           %featureArray = UtilityFunctions.RemoveDC(featureArray);
        end
        
    end
    
    methods(Static)
        
        %raèuna uniformno rasporeðene toèke (jedna toèka je na sredini slike)
        %numberOfPoints - traženi broj toèaka (neparan broj bi trebao biti)
        %points         - stupac vektor sa koordinatama toèaka [x y]
        function points = GetPoints(imageSize, numberOfPoints)
            points = []; numberOfPoints = sqrt(numberOfPoints); %gledamo samo za redak/stupac
            
            numOfSections = numberOfPoints;
            pointDistance = imageSize / numOfSections;
            
            pointCords = [];
            pointCord = 0;
            while numberOfPoints > 0
                pointCord = pointCord + pointDistance; 
                pointCords = cat(2, pointCords, pointCord);
               
                numberOfPoints = numberOfPoints -1;
            end
                     
            %uzmi za svaki red jednu koordinatu i...
            for pointIndxX = 1 : 1 : length(pointCords)
                pointCordX = pointCords(pointIndxX);            
                
                for pointIndxY = 1 : 1 : length(pointCords)
                    pointCordY = pointCords(pointIndxY);      
                    point = [pointCordX pointCordY];
                    
                    points = cat(1, points, point);              
                end             
            end   
            
        end 
        
        %responses - niz vektora znaèajki -> [brojSlika vektorZnaèajki] 
           %(vektorZnacajki -> odziv na tocku1, odziv na tocku2...)      
        function responses = ComputeResponses(imageArray, points, scale, frequency)
            responses = [];
            
            for idxOrinetation=1 : 1 : length(FeatureExtractor.GaborOrientations)
                orientation = FeatureExtractor.GaborOrientations(idxOrinetation);
                              
                for idxPoint=1 : 1 : length(points)
                    point = points(idxPoint, :);                   
                    responsesForPoint = FeatureExtractor.ComputeResponsesForPosition(imageArray, scale, orientation, frequency, point);

                    responses = cat(2, responses, responsesForPoint);
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
                filterResponse = UtilityFunctions.ScaleValues(filterResponse, 0, 255);
                filterResponse = sum(filterResponse);
                            
                responses = cat(1, responses, filterResponse);
            end
                    
        end
        
        function optimalFrequency = ChooseOptimalFrequency(responses, classes)
            %za svaku frekvenciju izraèunaj LDA -> odaberi onu koja
            %maksimizira LDA
        end
        
    end   
end