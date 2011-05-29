function [ min_inds ] = FindMin( vector, num )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

len = length(vector);

min_vals = [vector(1)];
min_inds = [1];

for i=2:len
    l = length(min_vals);
    for j=1:l
        if(vector(i) < min_vals(j))
            if(l < num)
                if j > 1
                    min_vals = [min_vals(1:j-1), vector(i), min_vals(j:l)];
                    min_inds = [min_inds(1:j-1), i, min_inds(j:l)];
                else
                    min_vals = [vector(i), min_vals(1:l)];
                    min_inds = [i, min_inds(1:l)];
                end
                
            else
                if j > 1
                    min_vals = [min_vals(1:j-1), vector(i), min_vals(j:l-1)];
                    min_inds = [min_inds(1:j-1), i, min_inds(j:l-1)];
                else
                    min_vals = [vector(i), min_vals(1:l-1)];
                    min_inds = [i, min_inds(1:l-1)];
                end
            end
            break; 
        end
    end
end

end

