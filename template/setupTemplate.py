import os
import shutil
import argparse


'''
    Run from the terminal like:
        > python ./template/setupTemplate.py 2022 9
'''

parser = argparse.ArgumentParser("template")
parser.add_argument("year", nargs='?', default="2022")
parser.add_argument("day", nargs='?', default="1")
args = parser.parse_args()

path = args.year + "/problem" + args.day + "/"
if not os.path.exists(path):
    os.mkdir(path)

cs_path = path + "problem" + args.day + ".cs"
shutil.copy("template/problem.txt", cs_path)

# Read in the file
with open(cs_path, 'r') as file:
  filedata = file.read()

# Replace the target string
filedata = filedata.replace('YEAR', args.year)
filedata = filedata.replace('DAY', args.day)

# Write the file out again
with open(cs_path, 'w') as file:
  file.write(filedata)

os.close(os.open(path + "testinput.txt", os.O_CREAT))
os.close(os.open(path + "input.txt", os.O_CREAT))