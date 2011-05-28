function img_adj = intensity_adjustment(img,c,gamma)
    mask=ones(5);
    mask=mask/sum(mask(:));
    img=double(img);
    img=conv2(img,mask,'same');
    img=c*(img.^gamma);
    img_adj=imscale(img,0,255);
    img_adj=uint8(img_adj);
return