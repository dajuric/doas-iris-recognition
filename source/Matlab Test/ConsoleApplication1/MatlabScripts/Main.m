function Main(img)

	for numberOfBits=1:(+3):4  
		numberOfLevels=2^numberOfBits-1;
		figure(  'Name',  strcat('Kvantizacija B=', num2str(numberOfBits))   );
		imshow(Quantize(img, numberOfLevels));
	end
	
end