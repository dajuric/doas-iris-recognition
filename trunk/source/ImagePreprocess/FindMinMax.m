function [ min max ] = FindMinMax( vector, ignore_first, ignore_last)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

len = length(vector);

max_value = 0;
max_index = 0;

min_value = 0;
min_index = 0;

for i=(ignore_first + 1):(len-ignore_last)
    if vector(i) < min_value
        min_value = vector(i);
        min_index = i;
    end
    
    if vector(i) > max_value
        max_value = vector(i);
        max_index = i;
    end
end

min = min_index;
max = max_index;

end

