:: Si previamente ya existia una carpeta con archivos comprimidos los borra.
@RD /S /Q "./Compressed/"
:: Llama a la herramienta 7zip y comprime todos los archivos dentro de la carpeta Externals, los separa en volumenes de 10mb como maximo.
"./7z Tool/7za.exe" -v10m a -tzip "./Compressed/Externals.zip" "./Assets/Externals/"