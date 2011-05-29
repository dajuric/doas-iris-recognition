classdef ImageLoader
    
    methods(Static)
        
        function [imgFileNames, classes] = LoadImagePaths(folderPath, minMaxFileIndex)
            imgFileNames = {}; classes = {};
            
            dirClasses = dir(folderPath);
            
            for i = 1 : 1 : length(dirClasses)
                %odredi klasu direktorija
                dirClassName = dirClasses(i).name;
                
                % zanemari '.' i '..' direktorije
                if (strcmp(dirClassName, '.') || strcmp(dirClassName, '..'))
                    continue;
                end 
                
                %izvadi putanja slika
                classDirFullName = fullfile(folderPath, dirClasses(i).name);                  
                imgClassFileNames = ImageLoader.GetFiles(classDirFullName, '.bmp');
                
                minIndex = minMaxFileIndex(1);
                maxIndex = minMaxFileIndex(2);
                
                if minIndex > length(imgClassFileNames)
                    error('nema dovoljno datoeka: poèetni index');                   
                end
                
                if maxIndex > length(imgClassFileNames)
                    error('nema dovoljno datoeka: krajnji index'); 
                end
                                
                %output: klase
                class = { ImageLoader.GetClass(dirClassName) };
                class = repmat( class, maxIndex - minIndex + 1, 1);
                classes =  cat(1, classes, class); 
                
                %output: imena slika (prvih numOfFilesToLoad slika)
                imgFileNames =  cat( 1, imgFileNames, imgClassFileNames(minIndex : maxIndex) );               
            end          
        end
        
    end
    
    methods(Static, Access = 'private') 
        
       function files = GetFiles(directory, fileExtension)
    
            contents    = dir(directory);
            directories = find([contents.isdir]);

            fileIndicies = find(~[contents.isdir]);    
            fileStructures = contents(fileIndicies);

            files = {};
            for i = 1 : 1 : length(fileStructures)
                fileName = fullfile(directory, fileStructures(i).name);

                %**************filter****************
                [folder, name, extension] = fileparts(fileName);
                if( strcmp(extension, fileExtension) )       
                    files = cat(1, files, fileName);
                end
            end

            % For loop will be skipped when directory contains no sub-directories
            for i_dir = directories

                sub_directory  = contents(i_dir).name;
                full_directory = fullfile(directory, sub_directory);

                % ignore '.' and '..'
                if (strcmp(sub_directory, '.') || strcmp(sub_directory, '..'))
                    continue;
                end 

                % Recurse down
                files = cat(1, files, ImageLoader.GetFiles(full_directory, fileExtension));
            end

        end
        
        %dirName - samo ime foldera (bez putanje)
        function class = GetClass(dirName)
            class = str2num(dirName); %rješi se vodeæih nula
            class = num2str(class);
        end
                    
    end
    
end