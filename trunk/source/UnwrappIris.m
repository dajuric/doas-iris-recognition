function unwrappedIris = UnwrappIris(img, cp, rp, ci, ri)
n=256.0;
segments=32;
dfi=2*pi/n;
cpx=double(cp(1));
cpy=double(cp(2));
cix=double(ci(1));
ciy=double(ci(2));
fi=0;
ri=double(ri);
rp=double(rp);
unwrappedIris=zeros(segments,n);
[h w]=size(img);
for i=1:n
    p1x=cpx+rp*cos(fi);
    p1y=cpy-rp*sin(fi);
    p2x=cpx+rp*cos(fi+dfi);
    p2y=cpy-rp*sin(fi+dfi);
    p3x=cix+ri*cos(fi);
    p3y=ciy-ri*sin(fi);
    p4x=cix+ri*cos(fi+dfi);
    p4y=ciy-ri*sin(fi+dfi);
    %figure;
    %imshow(img);
    %MarkPoint(p1x,p1y,'r');
    %MarkPoint(p2x,p2y,'r');
    %MarkPoint(p3x,p3y,'r');
    %MarkPoint(p4x,p4y,'r');
    for k=0:(segments-1)  %stavljeno je da bude linija debljine 32 bita, to se može mijenjati po volji 
        pax=p1x*(1-k/segments)+p3x*k/segments;
        pay=p1y*(1-k/segments)+p3y*k/segments;
        pbx=p1x*(1-(k+1)/segments)+p3x*(k+1)/segments;
        pby=p1y*(1-(k+1)/segments)+p3y*(k+1)/segments;
        pcx=p2x*(1-k/segments)+p4x*k/segments;
        pcy=p2y*(1-k/segments)+p4y*k/segments;
        pdx=p2x*(1-(k+1)/segments)+p4x*(k+1)/segments;
        pdy=p2y*(1-(k+1)/segments)+p4y*(k+1)/segments;
        minx=uint16(max(min([pax pbx pcx pdx]),1));
        miny=uint16(max(min([pay pby pcy pdy]),1));
        maxx=uint16(min(max([pax pbx pcx pdx]),w));
        maxy=uint16(min(max([pay pby pcy pdy]),h));
        submtr=img(miny:maxy,minx:maxx);
        avrgVal=mean2(submtr);
        unwrappedIris(k+1,i)=avrgVal;
    end
    fi=fi+dfi;
end
unwrappedIris=uint8(unwrappedIris);
return;