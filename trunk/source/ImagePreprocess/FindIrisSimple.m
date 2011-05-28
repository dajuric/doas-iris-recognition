function [cen rad] = FindIrisSimple(pupilCenter,pupilRadius, imgEdges)

x=round(pupilCenter(1));
y=round(pupilCenter(2));
pupilRadius=round(pupilRadius);


[imgw imgh]=size(imgEdges);
%find left border
left=0;
for i= 1:(x-pupilRadius-1)
    if(imgEdges(y,i)==1 && (x-i)>1.5*pupilRadius)
      left=i;
    end
end
%find right border
right=imgw;
for i=(x+pupilRadius+1) : imgw
    if(imgEdges(y,i)==1 && (i-x)>1.5*pupilRadius)
      right=i;
      break;
    end
end
%rad=min(x-left,right-x);
%cen=pupilCenter;

centerx=uint16((left+right)/2);
cen=zeros(2,1);
cen(1)=centerx;
cen(2)=y;
rad=uint16((right-left)/2);
if((y+rad)>imgh)
    rad=imgh-y;
end
if((y-rad)<0)
    rad=y;
end
