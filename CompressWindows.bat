:: Si previamente ya existia una carpeta con archivos comprimidos los borra.
@RD /S /Q "./Compressed/"
:: Llama a la herramienta 7zip y comprime todos los archivos dentro de la carpeta Externals, los separa en volumenes de 8mb como maximo.
"./7zip/7za.exe" -v8m a -t7z "./Compressed/Externals.7z" "./Assets/Externals/"