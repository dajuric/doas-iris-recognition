function [ center radius ] = FindPupil( image, map, coef)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

if(~exist('coef', 'var'))
    coef = 0.52;
end

if(isempty(coef))
    coef = 0.52;
end

dims = size(image);
if(length(dims) == 3)
    gray_image = rgb2gray(image);
    gray_map = rgb2gray(map);
    
    numOfPixels = dims(1) * dims(2);
    pixelSum = sum(gray_image);
    pixelSum = sum(pixelSum);
    thresh = coef * pixelSum / numOfPixels;
    binary_image = im2bw(gray_image, gray_map, thresh/255);
else
    gray_image = ind2gray(image, map); 
    numOfPixels = dims(1) * dims(2);
    pixelSum = sum(gray_image);
    pixelSum = sum(pixelSum);
    thresh = coef * pixelSum / numOfPixels
    binary_image = im2bw(gray_image, thresh/255);
end
% figure;
% imshow(binary_image);
proj_horiz = sum(binary_image, 1);
proj_vert = sum(binary_image, 2);

ignore_first = 0;
ignore_last = 0;
diff_horiz = diff(proj_horiz);
diff_vert = diff(proj_vert);

[minh maxh] = FindMinMax(diff_horiz, ignore_first, ignore_last);
[minv maxv] = FindMinMax(diff_vert, ignore_first, ignore_last);

center = zeros(2, 1);
rad1 = abs((maxh - minh) / 2);
rad2 = abs((maxv - minv) / 2);
center(1) = rad1 + minh;
center(2) = rad2 + minv;

if rad1 > rad2
   radius = rad1;
else
   radius = rad2;
end

end

