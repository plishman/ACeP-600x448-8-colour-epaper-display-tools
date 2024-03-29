# Some tools for processing images for use with the 7/8 colour ACeP and 4.2in black/white/red Waveshare displays (Windows)

## ProcessACeP
ProcessACeP uses imagemagick to process any image into an 8 colour dithered png, then into an ACeP file (raw 2px/byte format) for the ACeP colour epaper display. The palette provided to imagemagick is my best guess, and I think produces fairly good results (see how the black/white and colour gradients are reproduced on the display).

Note that the output image is rotated 90 degrees, since the program is intended for input images that are portrait rather than landscape format, and the ACeP display's native bitmaps are landscape format.

Copy the template/tools folder into the another folder, and place your images (any size/format supported by imagemagick) into a subfolder called "source" in the same folder. Then run ProcessACeP.exe. ProcessACeP.exe will output another file, processACeP.bat, which contains all of the commands to process the source images into ACeP files. Captions for Saint's feast days will be added from the file en-1962.txt if the filename of the source image is of the form <day>-<month>, eg. 14-9.jpg would be 14th September.

## Support for 4.2in black/white/red display with modified GxEPD display driver
The programs can also be used to dither black/white/red images in 8 shades of red and black, as supported by the modified GxEPD display driver used with the Lectionary project (https://github.com/plishman/Catholic-Lectionary-on-ESP8266/tree/master/libraries/GxEPD/GxGDEW042Z15).

### Sample black/white/ dithering output

#### Input
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/PentecostSunday-input.jpg" height="300"/>

#### Output
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/PentecostSunday-dithered.png" height="300"/>

#### Actual display
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/PentecostSunday-output.jpg" height="300"/>

### Sample black/white/red dithering output

#### Input
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/9-6-input.jpg" height="300"/>

#### Output
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/9-6-dithered.png" height="300"/>

#### Actual display
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/9-6-output.jpg" height="300"/>

## makeACeP
Turns any common image file format (png, jpg, gif etc) into a 600x448 pixel raw binary, in the native pixel format of the Waveshare ACeP display. Based on the program "converter" (https://github.com/cnlohr/epaper_projects/tree/master/atmega168pb_waveshare_color/tools/converter)

## ACePView
ACePView views ACeP files and outputs a png copy to the same directory. Use as a sanity check to make sure that the result of processing with ProcessACeP worked.

## Sample output

### Monochrome gradient (may show moire patterns in Github due to scaling, not present in fullsize image)

#### Input
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/bw-test-pattern-input.png" width="1000"/>

#### Output
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/bw-test-pattern-output.png" width="1000"/>

#### Actual display
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/bw-test-pattern-display.jpg" width="1000"/>

### Colour gradient

#### Input
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/colour-test-pattern-input.png" width="1000"/>

#### Output
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/colour-test-pattern-output.png" width="1000"/>

#### Actual display
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/colour-test-pattern-display.jpg" width="1000"/>

### Real image

#### Source image (input)
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/15-9.jpg" height="600"/>

#### Actual display
<img src="https://github.com/plishman/ACeP-600x448-8-colour-epaper-display-tools/blob/main/test-patterns/test-img-display.jpg" height="600"/>
