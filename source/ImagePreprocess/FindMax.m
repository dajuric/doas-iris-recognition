function [ max_inds ] = FindMax( vector, num )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

len = length(vector);

max_vals = [vector(1)];
max_inds = [1];

for i=2:len
    l = length(max_vals);
    for j=1:l
        if(vector(i) > max_vals(j))
            if(l < num)
                if j > 1
                    max_vals = [max_vals(1:j-1), vector(i), max_vals(j:l)];
                    max_inds = [max_inds(1:j-1), i, max_inds(j:l)];
                else
                    max_vals = [vector(i), max_vals(1:l)];
                    max_inds = [i, max_inds(1:l)];
                end
                
            else
                if j > 1
                    max_vals = [max_vals(1:j-1), vector(i), max_vals(j:l-1)];
                    max_inds = [max_inds(1:j-1), i, max_inds(j:l-1)];
                else
                    max_vals = [vector(i), max_vals(1:l-1)];
                    max_inds = [i, max_inds(1:l-1)];
                end
            end
            break;
        end
        
    end
end

end

