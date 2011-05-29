function [ min, max ] = FindMinMax( vector, n_extremes, toler)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here

len = length(vector);

max_index = FindMax(vector, n_extremes);
min_index = FindMin(vector, n_extremes);
center = len/2 - toler*len;

min_dist = len;
best_max_ind = -1;

for i=1:length(max_index)
   if(max_index(i) > center)
       if((max_index(i) - center) < min_dist)
           best_max_ind = i;
           min_dist = max_index(i) - center;
       end
   end
end

if best_max_ind < 0
    min = -1;
    max = -1;
    return;
end

max = max_index(best_max_ind);

min_dist = len;
best_min_ind = -1;



for i=1:length(min_index)
    if(min_index(i) < len/2)
        if((center - min_index(i)) < min_dist)
            best_min_ind = i;
            min_dist = center - min_index(i);
        end
    end
end

if(best_min_ind < 0)
    min = -1;
    max = -1;
    return;
end

min = min_index(best_min_ind);

end