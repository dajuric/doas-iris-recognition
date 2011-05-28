%dodaj putanje u put traženja
addpath( fullfile(pwd, 'ImagePreprocess') );
addpath( fullfile(pwd, 'FeatureExtractor') );


samplesPath = 'E:\CASIA_DATABASE_1.0\';
maxNumberOfSamplesPerClass = 5; %broj uzoraka za klasu (mapa 1 i mapa 2)

[imageNames, classes] = ImageLoader.LoadImagePaths(samplesPath, maxNumberOfSamplesPerClass);

unwrappedImages = {};
for idxImage = 1 : 1 : length(imageNames)
    unwrappedImages{idxImage} = TestDetection( imageNames{idxImage} );
    
    fprintf('Preprocesiram sliku: %d / %d', idxImage, length(imageNames) );
    %imshow(unwrappedImages{idxImage});
    pause;
end

