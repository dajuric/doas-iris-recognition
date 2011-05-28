%pomocna funkcija za izoštravanjem slike oduzimanjem zamuæene slike
function rez = mysharpen(img)
img=double(img);
mask=ones(5);
mask=mask/sum(mask(:));
temp=conv2(img,mask,'same');
rez=2*img-temp;
[rows,cols]=size(rez);
for i=1:rows
    for j=1:cols
        if(rez(i,j)>255) 
           rez(i,j)=255;
        end
        if(rez(i,j)<0) 
           rez(i,j)=0;
        end
    end
end
rez=uint8(rez);
return