# ShantaeText
Text Injecting and Extracting Software for Shantae on GBC.

Works with the USA version of the game.

The Extractor outputs a file named textBank.txt in the same folder of the .exe, which can be easily edited.

The Injector inserts in the ROM the edited textBank.txt file.

The injection is optimized.

It doesn't extract all the text, but most of it.

Missing text can be found around these locations: 0x10100, 0x285AB, 0x48AD0, 0x49370, 0x7C800, 0x23BFCE, 0x23C293.

Symbols can be paired up to a numeric value using the Localization table for the Injection phase. An example localization table is provided as Local.txt. It can be loaded up using Load Localization in the File dropdown menu.
In order to convert letters properly, you must put the character, a space and the tile number you want it to be converted to.
"è 61" will convert all instances of "è" in the textBank to tile number "61" when converting.

The textBank and the Localization table MUST have the same encoding, or thing will get weird.

If you need help, feel free to contact me at: https://twitter.com/Lorenzooone

If prompted the error "Error. Bank -x is too big" while injecting, it means your text is too big! Due to how the GB/GBC handles this with banks, we can't put the text in another place. "-x" tells you the bank of text that's too big. Entries from 0 to 3FFF are bank 1, entries from 4000 to 7FFF are bank 2 and entries from 8000 to BFFF are bank 3. Trimmer your text accordingly!
