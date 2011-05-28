function [ ] = TestDetection( image_name )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

[img map] = imread(image_name);

%[center2 radius2] = FindIris2(img, map, center, radius);
imgEnh=imgProcessForCircDetection(img,1,2);
[center radius] = FindPupil(img, map);
imgEdges=edge(imgEnh,'canny');
[center2 radius2]=FindIrisSimple(center,radius,imgEdges);

figure;
hold on;
imshow(img, map);
title(image_name);
MarkCircle(center, radius, 'r');
MarkCircle(center2, radius2, 'r');
figure;
unwrappedIris=UnwrappIris(img,center,radius,center2,radius2);
unwrappedIrisEnh=imgPreprocessForFeatures(unwrappedIris,1,1.5)
imshow(unwrappedIrisEnh);


end

