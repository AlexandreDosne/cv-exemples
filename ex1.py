###
# Code par Alexandre Dosne
###

import sys
from PIL import Image
from path import Path

d = Path(sys.argv[1])
pathToImages = Path(sys.argv[2])

print("Path: " + pathToImages)

def resize(toOpen):
    image = Image.open(toOpen)
    processedImage = image.resize((32, 32))
    processedImage.save("_output/" + toOpen)

for f in pathToImages.files("*.png"):
    print("Resizing " + f + " ...")
    a = f.split("\\")
    resize(a[len(a)-1])

print("Finished!")