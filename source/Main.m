cov(2,2)

clear;
fprintf('\n');

trainSamples = 'D:\uzorciMatlab\train\';
testSamples = 'D:\uzorciMatlab\test\';

fprintf('U�ENJE: U�itavam slike...\n');
[imageArrayTrain, classesTrain] = ImageLoader.LoadImages(trainSamples, 200);

fprintf('U�ENJE: Ekstrakcija zna�ajki...\n');
feTrain = FeatureExtractor(imageArrayTrain, classesTrain);
featureArrayTrain = feTrain.GetFeatures();



fprintf('TESTIRANJE: U�itavam slike...\n');
[imageArrayTest, classesTest] = ImageLoader.LoadImages(testSamples, 100);

fprintf('TESTIRANJE: Ekstrakcija zna�ajki...\n');
feTest = FeatureExtractor(imageArrayTest, classesTest);
featureArrayTest = feTest.GetFeatures();

fprintf('TESTIRANJE: Klasifikacija...\n');
classesCalculated = classify(featureArrayTest, featureArrayTrain, classesTrain); 

simmilarity = [];
for i = 1 : 1 : length(classesTest)
    similarity(i) = ( classesTest{i} == classesCalculated{i} );
end

accuracy = sum(similarity) / length(classesTest);
fprintf('TESTIRANJE: To�nost je: %f %%\n', accuracy*100);