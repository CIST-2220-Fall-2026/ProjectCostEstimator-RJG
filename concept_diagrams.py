from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


ROOT = Path(__file__).parent / 'Tutorials'
FONT = r'C:\Windows\Fonts\arial.ttf'
BOLD = r'C:\Windows\Fonts\arialbd.ttf'
BLUE = '#27348b'
TEAL = '#007e87'
LIGHT = '#eef3fb'
DARK = '#1a2638'


def font(size, bold=False):
    return ImageFont.truetype(BOLD if bold else FONT, size)


def centered(draw, box, title, detail):
    x1, y1, x2, y2 = box
    draw.rounded_rectangle(box, radius=22, fill=LIGHT, outline=BLUE, width=4)
    for text, y, face, color in (
        (title, y1 + 27, font(32, True), BLUE),
        (detail, y1 + 82, font(23), DARK),
    ):
        w = draw.textbbox((0, 0), text, font=face)[2]
        draw.text((x1 + (x2 - x1 - w) / 2, y), text, font=face, fill=color)


def arrow(draw, x1, y, x2, label=None):
    draw.line((x1, y, x2 - 18, y), fill=TEAL, width=8)
    draw.polygon(((x2, y), (x2 - 28, y - 17), (x2 - 28, y + 17)), fill=TEAL)
    if label:
        face = font(19)
        w = draw.textbbox((0, 0), label, font=face)[2]
        draw.text(((x1 + x2 - w) / 2, y + 20), label, font=face, fill=DARK)


def diagram(module, title, boxes, footer):
    image = Image.new('RGB', (1800, 480), 'white')
    draw = ImageDraw.Draw(image)
    draw.text((60, 28), title, font=font(37, True), fill=DARK)
    count = len(boxes)
    gap = 74
    left = 60
    width = (1680 - gap * (count - 1)) // count
    y1, y2 = 130, 305
    for i, (heading, detail) in enumerate(boxes):
        x1 = left + i * (width + gap)
        centered(draw, (x1, y1, x1 + width, y2), heading, detail)
        if i < count - 1:
            arrow(draw, x1 + width + 10, 217, x1 + width + gap - 10)
    draw.text((60, 365), footer, font=font(25), fill=DARK)
    output = ROOT / f'Module {module}' / f'module-{module}-concept.png'
    image.save(output)
    print(output)


diagram(8, 'Generated MVVM plumbing',
        [('Your code', 'State and decisions'), ('MVVM Toolkit', 'Generates properties and commands'), ('XAML', 'Binds to generated members')],
        'Validation and cost rules remain in the view model; the toolkit removes repetitive wiring.')

diagram(9, 'Keep UI work at the boundary',
        [('BOM view model', 'Makes item decisions'), ('Interaction interface', 'Defines edit and confirm results'), ('MAUI adapter', 'Opens pages and alerts')],
        'The adapter returns an edited item, null for Cancel, or a confirmation result.')

diagram(10, 'Save and reload the Bill of Materials',
        [('BOM view model', 'Requests a change'), ('IBomService', 'Defines async operations'), ('JsonBomService', 'Writes and loads JSON')],
        'A new app run loads the saved file; a failed save restores the collection.')

diagram(11, 'Build the app from its responsibilities',
        [('XAML views', 'Show pages and controls'), ('View models', 'Validate and decide'), ('Services', 'Persist and interact')],
        'Wire the layers through dependency injection after each part has a clear job.')
