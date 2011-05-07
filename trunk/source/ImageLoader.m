classdef ImageLoader
    
    methods(Static)
        
        function [imageArray, classes] = LoadImages(folderPath, maxFilesPerClass)
            imageArray = []; classes = {};
            
            imgFileNames = ImageLoader.GetImageFiles(folderPath, '.jpg');
            
            for i=1:1:length(imgFileNames)
                fileName = imgFileNames{i};
                
                loadedImagesPerClass = ImageLoader.DecomposeImage(fileName, maxFilesPerClass);
                imageArray = cat(1, imageArray, loadedImagesPerClass); %niži ih u vektor stupac
                             
                class = { ImageLoader.GetClass(fileName) };
                class = repmat( class, size(loadedImagesPerClass, 1), 1);
                classes =  cat(1, classes, class);    
            end 
            
            imageArray = double(imageArray);
        end
        
    end
    
    methods(Static, Access = 'private') 
        
        function imgFileNames = GetImageFiles(folderPath, pictureExtension)
            
             dirData = dir(folderPath);   %# Get the data for the current directory
             dirIndex = [dirData.isdir];  %# Find the index for directories
             fileList = {dirData(~dirIndex).name}';  %'# Get a list of the files
             if ~isempty(fileList)
                files = cellfun(@(x) fullfile(folderPath,x),...  %# Prepend path to files
                                   fileList,'UniformOutput',false);
             end
            
            for i=1:1:length(files) 
                fileName = files{i};
                [folder, name, extension] = fileparts(fileName);
                
                if extension==pictureExtension
                    imgFileNames{i} = fileName;
                end
            end
        end
        
        %filename - ime datoteke BEZ putanje
        function class = GetClass(fileName)
            [folder, name, extension] = fileparts(fileName);
            
            markerIndex = find(name=='_');
            class = name(markerIndex+1 : end);
        end
        
        %filename - ime datoteke BEZ putanje
        function imageArray = DecomposeImage(fileName, maxFilesPerClass)
            IMAGE_SIZE=28;
            imageArray = [];
            
            img=imread(fileName);  

            imgIndex=1;
            for x=1:IMAGE_SIZE:size(img, 2)
                for y=1:IMAGE_SIZE:size(img, 1)
                    
                    oneImage=img(y:y+IMAGE_SIZE-1, x:x+IMAGE_SIZE-1);
                    %imshow(oneImage);
                    if max(oneImage(:)) >= 50 %želim samo slike na kojima nešto piše
                        oneImage = reshape(  oneImage, 1, prod(size(oneImage))  ); %u jedan red
                        imageArray = cat(1, imageArray, oneImage); %vektor stupac
                             
                        if imgIndex >= maxFilesPerClass
                          return;
                        end
                        
                        imgIndex=imgIndex+1;
                    end
                    
                end
            end
            
        end       
        
    end
    
end