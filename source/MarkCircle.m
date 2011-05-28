function [ ] = MarkCircle( center, radius, color )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

MarkPoint(center(1), center(2), color);
rectangle('Position', [center(1) - radius, center(2) - radius, 2*radius, 2*radius], 'Curvature', [1, 1], 'EdgeColor', color);

end

