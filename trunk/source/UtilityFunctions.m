classdef UtilityFunctions
    
    methods(Static)
        
        function scaledVals = ScaleValues(matrix, newMin, newMax)
            matrix=double(matrix);

            oldMin = min(matrix(:));
            oldMax = max(matrix(:));

            if oldMax-oldMin == 0
                k=0;
            else
                k = (newMax - newMin) / (oldMax - oldMin);
            end

            scaledVals = k.*(matrix - oldMin);
        end
       
        function rez = RemoveDC(matrix)
            if isreal(matrix) == 0
                error('Unesite matricu sa realnim vrijednostima');
            end
            
            valueDC = mean( matrix(:) );
            rez = matrix - valueDC;
        end  
        
    end
    
end