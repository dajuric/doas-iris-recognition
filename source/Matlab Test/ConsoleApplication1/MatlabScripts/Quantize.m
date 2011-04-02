function newImage=Quantize(image, numberOfLevels)
    minVal=min(image(:));
    maxVal=max(image(:));
    
    quant=(maxVal-minVal)/numberOfLevels;
    
    newImage=round(image/quant) * quant;
end