function skalirano = imscale(slika, cmin, cmax)
% IMSCALE - Skalira ulaznu sliku
%   IMSCALE(I) vraca sliku velicine ulazne slike u kojoj
%   su svi elementi skalirani tako da su im vrijednosti
%   izmedu 0 i 1.
%
%   S = IMSCALE(I) vraæa S gdje je najveci element u
%   matrici S jedan, a najmanji nula.
%
%   S = IMSCALE(I, CLIM) gdje je CLIM = [CMIN CMAX]
%   vraca I linearno preslikan na interval CLIM (najveci
%   element je CMAX, a najmanj CMIN),
%
%   S = IMSCALE(I, CMIN, CMAX) vraca I linearno preslikan
%   na interval [CMIN CMAX]. 

% DOSl - Laboratorijske vjezbe - 14. 2. 2004.

% $Revizija: 1.0 $  $Datum: 2004/02/14 23:02:31 $
% $Autor(i): Tomislav Petkovic $

if 0 == nargin
   error('Morate zadati sliku.');
elseif 1 == nargin
   cmin = 0;
   cmax = 1;
elseif 2 == nargin
   cmax = cmin(2);
   cmin = cmin(1);
else
   cmin = cmin(1);
   cmax = cmax(1);
end

% pretvori sliku u double tip i odredi maksimum i minimum
slika = double(slika);
velicina = size(slika);
slika = reshape(slika, [prod(velicina) 1]);
smin = min(slika);
smax = max(slika);

% linearno preslikaj vrijednosti tako da stari minimum/maksimum
% postane novi minimum/maksimum
if 0 == smax-smin
   nagib = 0;
   pomak = min([cmin cmax]);
else
   nagib = (cmax-cmin) / (smax-smin);
   pomak = cmin - nagib*smin;
end
slika = slika*nagib + pomak;

skalirano = reshape(slika, velicina);
