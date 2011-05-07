cov(2,2)

clear;
fprintf('\n');

trainSamples = 'D:\uzorciMatlab\train\';
testSamples = 'D:\uzorciMatlab\test\';

fprintf('UÈENJE: Uèitavam slike...\n');
[imageArrayTrain, classesTrain] = ImageLoader.LoadImages(trainSamples, 200);

fprintf('UÈENJE: Ekstrakcija znaèajki...\n');
feTrain = FeatureExtractor(imageArrayTrain, classesTrain);
featureArrayTrain = feTrain.GetFeatures();



fprintf('TESTIRANJE: Uèitavam slike...\n');
[imageArrayTest, classesTest] = ImageLoader.LoadImages(testSamples, 100);

fprintf('TESTIRANJE: Ekstrakcija znaèajki...\n');
feTest = FeatureExtractor(imageArrayTest, classesTest);
featureArrayTest = feTest.GetFeatures();

fprintf('TESTIRANJE: Klasifikacija...\n');
classesCalculated = classify(featureArrayTest, featureArrayTrain, classesTrain); 

simmilarity = [];
for i = 1 : 1 : length(classesTest)
    similarity(i) = ( classesTest{i} == classesCalculated{i} );
end

accuracy = sum(similarity) / length(classesTest);
fprintf('TESTIRANJE: Toènost je: %f %%\n', accuracy*100);