#pragma once

static void trace_(const char * msg)
{
	FILE * f = fopen("trace.txt", "a");
	fprintf(f, msg);
	fprintf(f, "\n");
	fflush(f);
	fclose(f);
}