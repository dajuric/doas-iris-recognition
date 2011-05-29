classdef GaborKernel
   
  properties(GetAccess = 'public', SetAccess = 'private')
      KernelValues
  end
    
  methods      
        % matrixSize  - Velièina matrice (poželjno neparan broj zbog središta)
        % sigma       - Faktor za skaliranje filtra. Veæa vrijednost, veæi filtar.
        % theta       - Kut zakreta gaborovog filtra (u  radijanima)
        % f           - Frekvencija titranja sinusnog signala (u prostornoj domeni)
        % centerPoint - toèka središta filtra [x y]
        function this = GaborKernel(matrixSize, sigma, theta, f, centerPoint)
    
          r = 2; %konstanta
            
          pointX = centerPoint(1); 
          pointY = centerPoint(2);
          
          maxX = matrixSize - pointX;
          maxY = matrixSize - pointY;
          
          if(mod(matrixSize, 2) ==0) %ako je matrica parnih dimenzija
              maxX = maxX - 1; %kernel mora biti paran
              maxY = maxY - 1;
          end

          e = exp(1); 
          kernel=zeros(matrixSize, matrixSize);
          
          for x = -pointX :1: maxX
             for y = -pointY :1: maxY
                
               R1 = x*cos(theta) + y*sin(theta);
               R2 =-x*sin(theta) + y*cos(theta);
                 
               expFactor = -1/2 * ( (R1/sigma)^2 + (R2/(r*sigma))^2  );
               
               gauss = 1 / ( sqrt(r*pi)*sigma) ;
               gauss =  gauss * e^expFactor;
                 
               gaborReal = gauss * cos(f*R1);
               gaborImag = gauss * sin(f*R1);
               
               kernel(y + pointY +1, x + pointX +1) = gaborReal + gaborImag*i;
             end 
          end

          this.KernelValues = GaborKernel.RemoveDC(kernel);
        end
        
        function scaledAmp = GetAmplitudes(this)
            realPart = real(this.KernelValues);
            imagPart = imag(this.KernelValues);
            
            amp = sqrt( realPart.^2 + imagPart.^2 );
            scaledAmp = UtilityFunctions.ScaleValues(amp, -1, 1);
            %scaledAmp = amp;
        end

        function scaledRealPart = GetRealParts(this)
            realPart = real(this.KernelValues);          
            scaledRealPart = UtilityFunctions.ScaleValues(realPart, -1, 1);
            %scaledRealPart = realPart;
        end
        
        function scaledImagPart = GetImagParts(this)
            imagPart = imag(this.KernelValues);          
            scaledImagPart = UtilityFunctions.ScaleValues(imagPart, -1, 1);
            %scaledImagPart = imagPart;
        end
  end
  
  methods(Static, Access = 'private')
      
        function rez = RemoveDC(matrix)
           
            valueDC = mean( real(matrix(:)) ); %imaginarna vrijednost nema DC komponentu
            rez = matrix - valueDC;
        end  
  end
  
end