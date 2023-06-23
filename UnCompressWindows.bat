:: Aca se llama a la herramienta 7zip para que descomprima todos los primeros archivos de grupos por volumenes.
".\7z Tool\7za.exe" x -y ".\Compressed\**.zip.001" -o".\Assets\"
::@RD /S /Q "./Compressed/"  Esta linea la comente pero basicamente una vez termina de descomprimir borra todos los archivos.