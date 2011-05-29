%poboljšanje slike kako bi se lakše mogla raditi detekcija zjenice i
%šarenice, NE KORISTITI tako dobivenu sliku kasnije kod ekstrakcije
%znaèajki jer se ovim postupkom izbrišu sve linije u šarenici!
%na ovako dobivenim slikama jako lijepo mi je cannyjev filtar nalazio
%rubove zjenice i šarenice :)

%što se tièe parametara za konstantu c nisam vidio da pretjerano utjeèe na
%rezultat tako da nije nešto bitna, dok gamma je dobra negdje izmeðu 1.5 i
%2 uglavnom, pa kušt može probati malo viditi koja vrijednost njemu
%najbolje paše za njegov postupak detekcije 

%ukratko ovo je predprocesiranje za kušta
function img_adj = imgProcessForCircDetection(img,c,gamma)    
    img=medfilt2(img,[20 20],'symmetric');
    img=double(img);
    img=c*(img.^gamma);
    img_adj=imscale(img,0,255);
    img_adj=uint8(img_adj);
return