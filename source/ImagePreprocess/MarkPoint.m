function [ ] = MarkPoint( x, y, color)
%UNTITLED2 Method marks given point on the current plot
%   Detailed explanation goes here


x1 = x - 3;
y1 = y - 3;
x2 = x + 4;
y2 = y + 4;

line([x1, x2], [y1, y2], 'Color', color);
line([x2, x1], [y1, y2], 'Color', color);


end

