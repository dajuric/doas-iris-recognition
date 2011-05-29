function [ center radius ] = FindPupil( image, map, coef, n_extremes, toler)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

if(~exist('coef', 'var'))
    coef = 0.3;
end

if(isempty(coef))
    coef = 0.3;
end

if(~exist('n_extremes', 'var'))
    n_extremes = 3;
end

if(isempty(n_extremes))
    n_extremes = 3;
end

if(~exist('toler', 'var'))
    toler = 0.14;
end

if(isempty(toler))
    toler = 0.14;
end

dims = size(image);
if(length(dims) == 3)
    gray_image = rgb2gray(image);
    gray_image = medfilt2(gray_image, [15, 15]);
    gray_map = rgb2gray(map);
    
    numOfPixels = dims(1) * dims(2);
    pixelSum = sum(gray_image);
    pixelSum = sum(pixelSum);
    thresh = coef * pixelSum / numOfPixels;
    binary_image = im2bw(gray_image, gray_map, thresh/255);
else
    gray_image = ind2gray(image, map); 
    gray_image = medfilt2(gray_image, [15, 15]);
    numOfPixels = dims(1) * dims(2);
    pixelSum = sum(gray_image);
    pixelSum = sum(pixelSum);
    thresh = coef * pixelSum / numOfPixels;
    binary_image = im2bw(gray_image, thresh/255);
end
% figure;
% imshow(binary_image);
proj_horiz = sum(binary_image, 1);
proj_vert = sum(binary_image, 2);

diff_horiz = diff(proj_horiz);
diff_vert = diff(proj_vert);

%figure;
%hold on;
%subplot(2, 1, 1); plot(diff_horiz); title('horiz');
%subplot(2, 1, 2); plot(diff_vert); title('vert');
%figure;
%hold off;

[minh maxh] = FindMinMax(diff_horiz, n_extremes, toler);
[minv maxv] = FindMinMax(diff_vert, n_extremes, toler);

if(minh < 0 || minv < 0 || maxh < 0 || maxv < 0)
    radius = -1;
    center = [];
    return;
end

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

