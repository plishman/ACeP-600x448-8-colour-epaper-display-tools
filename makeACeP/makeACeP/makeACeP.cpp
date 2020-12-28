#include <stdio.h>
#include <math.h>

#include <stdint.h>

#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"

int depalette(uint8_t * color, uint8_t* palette, int palette_size);
int depalette(uint8_t * color);

/*
uint8_t palette[8*3] = {
	0, 0, 0,
	255, 255, 255,
	67, 138, 28,
	100, 64, 255,
	191, 0, 0,
	255, 243, 56,
	232, 126, 0,
	194 ,164 , 244 
};
*/

/*
uint8_t palette[8 * 3] = {
	0, 0, 0,
	255, 255, 255,
	67, 138, 28,
	36, 36, 255,
	191, 0, 0,
	255, 243, 56,
	232, 126, 0,
	235, 164, 244,
};
*/

uint8_t palette[8 * 3] = {
	0, 0, 0,
	255, 255, 255,
	67, 138, 28,
	36, 36, 255,
	191, 0, 0,
	255, 243, 56,
	232, 126, 0,
	235, 164, 244,
};

/*
uint8_t palette[8 * 3] = {
	0, 0, 0,
	255, 255, 255,
	0, 255, 0,
	0, 0, 255,
	255, 0, 0,
	255, 255, 0,
	255, 128, 0,
	255, 128, 255 //255, 0, 128,
};
*/

uint8_t palette_mono_8[8 * 3] = {
	255, 255, 255,	// white = 0
	222, 222, 222,
	190, 190, 190,
	148, 148, 148,
	116, 116, 116,
	76, 76, 76,
	42, 42, 42,
	0, 0, 0,
};

/*
uint8_t palette_black_red_8_8[16 * 3] = {
	255, 255, 255,	// white == 0
	222, 222, 222,
	190, 190, 190,
	148, 148, 148,
	116, 116, 116,
	76, 76, 76,
	42, 42, 42,
	0, 0, 0,
	255, 255, 255,
	255, 214, 214,
	255, 181, 181,
	255, 148, 148,
	255, 107, 107,
	255, 76, 76,
	255, 33, 33,
	255, 0, 0,
};
*/

uint8_t palette_black_red_8_8[16 * 3] = {
	255, 255, 255,	// white == 0
	205, 205, 205,
	164, 164, 164,
	96, 96, 96,
	58, 58, 58,
	25, 25, 25,
	0, 0, 0,
	0, 0, 0,
	255, 255, 255,
	237, 151, 151,
	237, 118, 118,
	237, 118, 118,
	237, 64, 64,
	237, 64, 64,
	255, 0, 0,
	237, 0, 0,
};

/*
uint8_t palette_black_red_8_8[16 * 3] = {
	255, 255, 255,	// white == 0
	205, 205, 205,
	125, 125, 125,
	79, 79, 79,
	51, 51, 51,
	19, 19, 19,
	4, 4, 4,
	0, 0, 0,
	255, 255, 255,
	255, 211, 211,
	255, 166, 166,
	255, 166, 166,
	255, 133, 133,
	255, 90, 90,
	255, 45, 45,
	255, 0, 0,
};
*/
/*
uint8_t palette_black_red_8_8[16 * 3] = {
	255, 255, 255,	// white == 0
	204, 205, 205,
	125, 125, 124,
	79, 78, 79,
	51, 51, 50,
	19, 19, 18,
	4, 4, 5,
	0, 0, 0,
	255, 255, 255,
	255, 211, 212,
	255, 166, 167,
	255, 166, 167,
	255, 133, 133,
	255, 89, 90,
	255, 45, 45,
	255, 0, 0,
};
*/



int numpaletteentries = 8;
/*
void readpalette()
{
	FILE * fin = fopen(".\\acep_palette.txt", "r");
	int paletteentry = 0;

	int p_entry = 0;
	char comment[1024];

	int r = 0;
	int g = 0;
	int b = 0;

	if (fin != NULL) {
		numpaletteentries = 0;
	}
	else {
		return;
	}

	while (fscanf(fin, "%d=#%2x%2x%2x\t%s\r\n", &p_entry, &r, &g, &b, comment) == 5) { // expect 3 successful conversions
		if (p_entry < 8) {
			palette_in[(p_entry * 3) + 0] = (uint8_t)r;
			palette_in[(p_entry * 3) + 1] = (uint8_t)g;
			palette_in[(p_entry * 3) + 2] = (uint8_t)b;

			if (numpaletteentries < p_entry) {
				numpaletteentries = p_entry;
			}
		}

		char c = '\0';
		while ((c = fgetc(fin)) != EOF && c != '\n') {}
	}

	numpaletteentries++;
	fclose(fin);
}
*/

int depalette( uint8_t * color, uint8_t* palette, int palette_size, bool bGreyScale )
{
	int p;
	int mindiff = 100000000;
	int bestc = 0;

	uint8_t r = color[0];
	uint8_t g = color[bGreyScale ? 0 : 1];
	uint8_t b = color[bGreyScale ? 0 : 2];

	for( p = 0; p < palette_size / 3 /*sizeof(palette)/3*/ /*numpaletteentries*/; p++ )
	{
		uint8_t pal_r = palette[p * 3 + 0];
		uint8_t pal_g = palette[p * 3 + 1];
		uint8_t pal_b = palette[p * 3 + 2];

		int diffr = ((int)r) - ((int)pal_r);
		int diffg = ((int)g) - ((int)pal_g);
		int diffb = ((int)b) - ((int)pal_b);
		int diff = (diffr*diffr) + (diffg*diffg) + (diffb * diffb);
		if( diff < mindiff )
		{
			mindiff = diff;
			bestc = p;
		}
	}
	return bestc;
}

int depalette(uint8_t * color) {
	return depalette(color, palette, sizeof(palette), false);	// default to ACeP palette
}

/*
int map(uint8_t * color) {
	// remap from input dithered image to epaper palette - 1 to 1 mapping, 7 or 8 colours (but should work with fewer)
	// the input palette is known, being read from the text file tools\acep_palette.txt. This is the same palette as is used in the remap file acep_palette.gif
	// passed to imagemagick to do the colour reduction and dithering

	// find out which colour the rgb triple corresponds with
	int p = 0;

	while (p < numpaletteentries) {
		if (color[0] == palette_in[(p * 3) + 0] && color[1] == palette_in[(p * 3) + 1] && color[2] == palette_in[(p * 3) + 2]) {
			return p;
		}
		p++;
	}
	return depalette(color); // colour not found, return nearest
}
*/

#define PALETTE_ACEP 0
#define PALETTE_BW 1
#define PALETTE_BWR 2

int main( int argc, char ** argv )
{
	if( argc != 3 && argc != 4 )
	{
		fprintf( stderr, "Usage: makeACeP [image file] [out file] [[ACeP|A|B]]\n" );
		fprintf( stderr, "Optional parameter:\n");
		fprintf( stderr, "(ACeP => use 8 colour ACeP epaper palette\n A => b/w palette (8 levels)\n B => b/w/r palette (8 levels of grey, red)\n");
		fprintf( stderr, "(defaults to ACeP palette if parameter not supplied)\n");
		return -1;
	}

	int palette_number = PALETTE_ACEP;
	int display_width = 600;
	int display_height = 448;

	if (argc == 4) 
	{
		if (strcmp(argv[3], "A") == 0) palette_number = PALETTE_BW;
		if (strcmp(argv[3], "B") == 0) palette_number = PALETTE_BWR;

		if (palette_number != PALETTE_ACEP) {
			display_width = 400;
			display_height = 300; // expect 4.2" 400x300 display if palette A or B is selected
		}
	}

	int x,y,n;
	unsigned char *data = stbi_load(argv[1], &x, &y, &n, 0);
	if( !data )
	{
		fprintf( stderr, "Error: Can't open image.\n" );
		return -6;
	}

	char outfilename[2048];  // leave room for long paths etc.
	if (strlen(argv[2]) > sizeof(outfilename) - 10) {	// -10 to leave room to concatenate file extensions for output images (.ACeP, .bw or .bwr)
		fprintf(stderr, "Error file/pathname too long.\n");
		return -20;
	}

	strcpy(outfilename, argv[2]);

	switch (palette_number) {
	case PALETTE_ACEP:
		if (x != 600) // expect either 600x448px or 400x300px image (will truncate if taller)
		{
			fprintf(stderr, "Error: image dimensions must be 600 x ?? for ACeP palette.\n");
			return -2;
		}
		strcat(outfilename, ".ACeP");
		break;

	case PALETTE_BW:
		if (x != 400) // expect either 600x448px or 400x300px image (will truncate if taller)
		{
			fprintf(stderr, "Error: image dimensions must be 400 x ?? for bw/bwr palette (for 4.2in display)\n");
			return -2;
		}
		
		strcat(outfilename, ".bw");
		break;

	case PALETTE_BWR:
		if (x != 400) // expect either 600x448px or 400x300px image (will truncate if taller)
		{
			fprintf(stderr, "Error: image dimensions must be 400 x ?? for bw/bwr palette (for 4.2in display)\n");
			return -2;
		}

		strcat(outfilename, ".bwr");
		break;

	default:
		fprintf(stderr, "Error: palette not chosen (should not happen)\n");
		return -10;
		break;
	}

	if (y > display_height)
	{
		y = display_height;
	}

	int i, j;
	FILE * fout = fout = fopen(outfilename, "wb");

	int margin = display_height - y;
	uint8_t line[600 / 2];

	int psize = sizeof(palette);
	uint8_t* pPalette = palette;

	bool bGreyScale = (n == 1); // 1 byte/pixel input format if so (rather than rgb triples)

	switch (palette_number) {
	case PALETTE_ACEP:	
		if( y < display_height )
		{
			int k;
			int ke = margin / 2;
			//memset( line, 0x66, 600/2 );
			memset( line, 0x11, 600/2 );
			for( k = 0; k < ke; k++ )
			{
				fwrite( line, 600/2, 1, fout );
			}
			margin -= ke;
		}
		else
		{
		}

		if (y > display_height) y = display_height;
		for (j = 0; j < y; j++)
		{
			for (i = 0; i < x / 2; i++)
			{
				int c1 = depalette(data + n * (i * 2 + x * j));
				int c2 = depalette(data + n * (i * 2 + x * j + 1));
				//int c1 = map(data + n * (i * 2 + x * j));
				//int c2 = map(data + n * (i * 2 + x * j + 1));
				line[i] = c2 | (c1 << 4);
			}
			fwrite(line, 600 / 2, 1, fout);
		}
		int k;
		//memset( line, 0x66, 600/2 );
		memset(line, 0x11, 600 / 2);
		for (k = 0; k < margin; k++)
		{
			fwrite(line, 600 / 2, 1, fout);
		}
		stbi_image_free(data);
		break;

	case PALETTE_BW:
	case PALETTE_BWR:
		/* // no borders
		if (y < display_height)
		{
			int ke = margin / 2;
			//memset( line, 0x66, 600/2 );
			memset(line, 0x00, display_width / 2);	// 0x00 = white
			for (int k = 0; k < ke; k++)
			{
				fwrite(line, display_width / 2, 1, fout);
			}
			margin -= ke;
		}
		else
		{
		}
		*/

		if (y > display_height) y = display_height;
		
		if (palette_number == PALETTE_BW) {
			psize = sizeof(palette_mono_8);
			pPalette = palette_mono_8;
		}
		else {	// must be PALETTE_BWR
			psize = sizeof(palette_black_red_8_8);
			pPalette = palette_black_red_8_8;
		}

		for (j = 0; j < y; j++)
		{
			for (i = 0; i < x / 2; i++)
			{
				int c1 = depalette(data + n * (i * 2 + x * j), pPalette, psize, bGreyScale);
				int c2 = depalette(data + n * (i * 2 + x * j + 1), pPalette, psize, bGreyScale);
				line[i] = c2 | (c1 << 4);
			}
			fwrite(line, display_width / 2, 1, fout);
		}
		/*	// no borders
		memset(line, 0x00, display_width / 2);
		for (int k = 0; k < margin; k++)
		{
			fwrite(line, display_width / 2, 1, fout);
		}
		*/
		stbi_image_free(data);
		break;

	default:
		break;
	}
	fclose(fout);
}



