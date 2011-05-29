clear;

%dodaj putanje u put traženja
addpath( fullfile(pwd, 'ImagePreprocess') );
addpath( fullfile(pwd, 'FeatureExtractor') );


samplesPath = 'E:\CASIA_DATABASE_1.0\';

%3-1+1 = 3 primjera po klasi (broj uzoraka za klasu (mapa 1 i mapa 2))
[imageNamesTrain, classesTrain] = ImageLoader.LoadImagePaths(samplesPath, [1 3]);  

unwrappedImagesTrain = [];
for idxImage = 1 : 1 : length(imageNamesTrain)
    unwrappedImage = TestDetection( imageNamesTrain{idxImage} );
    unwrappedImagesTrain = cat(1, unwrappedImagesTrain, unwrappedImage);
    
    fprintf('UÈENJE: Preprocesiram sliku: %d / %d', idxImage, length(imageNamesTrain) );
    %imshow(unwrappedImages{idxImage});
    pause; %za gledanje oznaèenih slika
end

feExtractorTrain = FeatureExtractor(256, 32);
featureVectorArrayTrain = feExtractorTrain.ExtractFeatures(unwrappedImagesTrain);


%3-1+1 primjera po klasi (broj uzoraka za klasu (mapa 1 i mapa 2))
[imageNamesTest, classesTest] = ImageLoader.LoadImagePaths(samplesPath, [4 5]);  

unwrappedImagesTest = [];
for idxImage = 1 : 1 : length(imageNamesTest)
    unwrappedImage = TestDetection( imageNamesTest{idxImage} );
    unwrappedImagesTest = cat(1, unwrappedImagesTest, unwrappedImage);
    
    fprintf('TESTIRANJE: Preprocesiram sliku: %d / %d', idxImage, length(imageNamesTest) );
    %imshow(unwrappedImages{idxImage});
    pause;%za gledanje oznaèenih slika
end

feExtractorTest = FeatureExtractor(256, 32);
featureVectorArrayTest = feExtractorTest.ExtractFeatures(unwrappedImagesTest);

%load('variables.mat');

%lda = LDA(featureVectorArrayTrain, classesTrain);
%lda.Compute();
%fATr = lda.Transform(featureVectorArrayTrain, lda.NumberOfClasses -1);
%fATe = lda.Transform(featureVectorArrayTest, lda.NumberOfClasses -1); 
%classesCalculated = classify(fATe, fATr, classesTrain); 

classesCalculated = knnclassify(featureVectorArrayTest, featureVectorArrayTrain, classesTrain, 1); 

%classesCalculated = classify(featureVectorArrayTest, featureVectorArrayTrain, classesTrain); 

simmilarity = [];
for i = 1 : 1 : length(classesTest)
    similarity(i) = strcmp(classesTest{i},classesCalculated{i}) ;
end

accuracy = sum(similarity) / length(classesTest);
fprintf('TESTIRANJE: Toènost je: %f %%\n', accuracy*100);