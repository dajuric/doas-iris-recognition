%poveæanje kontrasta za Darka tako da se bolje vide linije u šarenici
%isto kao i za drugu funkciju vrijedi da c ne igra neku uloga, a gamma
%negdje oko 1.5 je ok po meni

%ovo šta sam zakomentirao je funkcija koja dodatno izoštrava sliku, ali
%onda je slika dosta zrnatija, pa neznam da li to smeta ili ne. probaj sa i
%bez pa vidi da li i kako utjeèe to dodatno izoštravanje na rezultate
function img_adj = imgProcessForFeatures(img,c,gamma)
    %img=mysharpen(img);
    img=double(img);
    img=c*(img.^gamma);
    img_adj=imscale(img,0,255);
    img_adj=uint8(img_adj);
return