call C:\Users\11011105\Anaconda3\condabin\activate.bat
call conda activate py38
set allArguments=%*
cd %~dp0
python ctf.py %allArguments%