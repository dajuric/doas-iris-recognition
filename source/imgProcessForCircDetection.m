%pobolj�anje slike kako bi se lak�e mogla raditi detekcija zjenice i
%�arenice, NE KORISTITI tako dobivenu sliku kasnije kod ekstrakcije
%zna�ajki jer se ovim postupkom izbri�u sve linije u �arenici!
%na ovako dobivenim slikama jako lijepo mi je cannyjev filtar nalazio
%rubove zjenice i �arenice :)

%�to se ti�e parametara za konstantu c nisam vidio da pretjerano utje�e na
%rezultat tako da nije ne�to bitna, dok gamma je dobra negdje izme�u 1.5 i
%2 uglavnom, pa ku�t mo�e probati malo viditi koja vrijednost njemu
%najbolje pa�e za njegov postupak detekcije 

%ukratko ovo je predprocesiranje za ku�ta
function img_adj = imgProcessForCircDetection(img,c,gamma)    
    img=medfilt2(img,[20 20],'symmetric');
    img=double(img);
    img=c*(img.^gamma);
    img_adj=imscale(img,0,255);
    img_adj=uint8(img_adj);
return