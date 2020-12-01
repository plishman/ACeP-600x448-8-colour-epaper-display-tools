rem Do Dithering
mkdir ..\dithered
del ..\dithered\*.png /Q
magick.exe "..\source\test.png" -rotate "90" -gamma 0.454545 -resize 600x448 -background White -gravity center -extent 600x448 -dither FloydSteinberg -remap ".\acep_palette.gif" "..\dithered\test.png"
magick.exe "..\source\test4.png" -rotate "90" -gamma 0.454545 -resize 600x448 -background White -gravity center -extent 600x448 -dither FloydSteinberg -remap ".\acep_palette.gif" "..\dithered\test4.png"
rem Make ACeP
mkdir ..\ACeP
del ..\ACeP\*.* /Q
copy .\ACepView.exe ..\ACeP
.\makeACeP.exe "..\dithered\test.png" "..\ACeP\test.ACeP"
.\makeACeP.exe "..\dithered\test4.png" "..\ACeP\test4.ACeP"
del ..\dithered\*.caption.txt
