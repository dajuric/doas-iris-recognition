classdef FilterBank_Extractor
    
    properties
        Frequencies = [2, 4, 8, 16, 32];
        Orientations = [0, pi/4, pi/2, 3*pi/4];
        Scale = [4];
    end
    
    properties
        ImageArray,
        ImageSize
    end
    
    methods(Access = 'public')
        
        function this = FilterBank_Extractor(imageArray)
            this.ImageArray = imageArray;
            this.ImageSize = size(imageArray, 2); %slika je kvadratna
        end
        
        
        function featureArray = GetFeatures(this)                      
            %numOfImages = size(this.ImageArray, 1) / this.ImageSize;
            
            featureArray = [];
            
            for imagePos = 1 : this.ImageSize : size(this.ImageArray, 1)
                
                %slike su poredane u vektor stupac
                image = this.ImageArray(imagePos : imagePos + this.ImageSize-1, :);
                
                featureVector = this.GetFeatureVector(image);
                
                %sla�emo vektore u retke
                featureArray = cat(1, featureArray, featureVector);
            end
            
        end
        
        function featureVector = GetFeatureVector(this, image)
            
            featureVector = [];
            
            gaborPosition = [this.ImageSize/2 this.ImageSize/2];
            
            for idxFreq = 1 : 1 : length(this.Frequencies)              
                frequency = this.Frequencies(idxFreq);
                
                for idxOrientation = 1 : 1 : length(this.Orientations)                  
                    orientation = this.Orientations(idxOrientation);
                    
                    g = GaborKernel(this.ImageSize, this.Scale, orientation, frequency, gaborPosition);
                    feature = this.GetFeature(g.GetImagParts(), image);
                    
                    featureVector = cat(2, featureVector, feature);
                end             
            end
            
        end
        
        function feature = GetFeature(this, gaborImage, image)
            convolvedIm = conv2(double(gaborImage), double(image), 'same');
            
            imageMean = mean(convolvedIm(:));
            
            feature = abs(convolvedIm - imageMean);
            feature = sum(feature(:)) / prod(size(convolvedIm));
        end
        
    end
    
end