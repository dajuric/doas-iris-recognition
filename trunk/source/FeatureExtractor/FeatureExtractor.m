classdef FeatureExtractor
    
    properties
        ImageWidth,
        ImageHeight
    end
    
    methods(Access = 'public')
        
        function this = FeatureExtractor(imageWidth, imageHeight)
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        end
        
        function featureVectors = ExtractFeatures(this, unwrappedImages)
            
            featureVectors =  [];
            
            numberOfParts = this.ImageWidth / this.ImageHeight;
            
            if(numberOfParts ~= floor(numberOfParts) )
                numberOfParts = floor(numberOfParts);
                warning('unwrappedImage se ne može podijeliti cjelobrojno...');
            end;
            
            for idxPart = 1 : 1 : numberOfParts
                fprintf('    Obraðujem %d/%d pravokutnih dijelova svih slika.\n', idxPart, numberOfParts);
                
                imagesPart = this.GetImagesPart(unwrappedImages, idxPart);
                
                fbExtractor = FilterBank_Extractor(imagesPart);               
                featureVectorForPart = fbExtractor.GetFeatures();
                
                %samo ih lijepimo da bude jedan veliki fetureVector
                featureVectors = cat(2, featureVectors, featureVectorForPart);
            end       
            
        end
        
        %imagePartIdx - [1..numberOfParts-1]
        function imagesPart = GetImagesPart(this, unwrappedImages, imagePartIdx)
            
            partStartPos = (imagePartIdx-1) * this.ImageHeight + 1; 
            partEndPos = partStartPos + this.ImageHeight - 1;
            
            imagesPart = unwrappedImages(:, partStartPos:partEndPos);                      
        end
        
    end
    
end