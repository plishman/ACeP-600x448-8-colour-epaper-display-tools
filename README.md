#Some tools for processing images for use with the 7/8 colour Waveshare ACeP display

#ProcessACeP
ProcessACeP uses imagemagick to process any image into an 8 colour dithered png, then into an ACeP file (raw 2px/byte format) for the ACeP colour epaper display. The palette provided to imagemagick is my best guess, and I think produces fairly good results (see how the black/white and colour gradients are reproduced on the display).

Copy the template/tools folder into the another folder, and place your images (any size/format supported by imagemagick) into a subfolder called "source" in the same folder. Then run ProcessACeP.exe. ProcessACeP.exe will output another file, processACeP.bat, which contains all of the commands to process the source images into ACeP files. Captions for Saint's feast days will be added from the file en-1962.txt if the filename of the source image is of the form <day>-<month>, eg. 14-9.jpg would be 14th September.

#makeACeP
Turns any common image file format (png, jpg, gif etc) into a 600x448 pixel raw binary, in the native pixel format of the Waveshare ACeP display. Based on the program "converter" (https://github.com/cnlohr/epaper_projects/tree/master/atmega168pb_waveshare_color/tools/converter)

#ACePView
ACePView views ACeP files and outputs a png copy to the same directory. Use as a sanity check to make sure that the result of processing with ProcessACeP worked.