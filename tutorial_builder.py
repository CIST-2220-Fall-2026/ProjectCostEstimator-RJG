from pathlib import Path
from docx import Document
from docx.shared import Inches, Pt
from docx.enum.text import WD_BREAK
from docx.oxml import OxmlElement
from docx.oxml.ns import qn

ROOT = Path(__file__).parent
OUT = ROOT / 'Tutorials'
OUT.mkdir(exist_ok=True)

def build(number, title, introduction, steps, files, references):
    folder = OUT / f'Module {number}'
    folder.mkdir(parents=True, exist_ok=True)
    doc = Document()
    section = doc.sections[0]
    section.page_width = Inches(8.5)
    section.page_height = Inches(11)
    section.top_margin = section.bottom_margin = Inches(.7)
    section.left_margin = section.right_margin = Inches(.85)
    styles = doc.styles
    styles['Normal'].font.name = 'Aptos'
    styles['Normal'].font.size = Pt(10.5)
    styles['Normal'].paragraph_format.space_after = Pt(6)
    for name, size in [('Title', 21), ('Heading 1', 15), ('Heading 2', 12)]:
        styles[name].font.name = 'Aptos Display' if name == 'Title' else 'Aptos'
        styles[name].font.size = Pt(size)
        styles[name].font.color.rgb = __import__('docx').shared.RGBColor(0, 0, 0)
    title_ppr = styles['Title'].element.get_or_add_pPr()
    borders = OxmlElement('w:pBdr')
    bottom = OxmlElement('w:bottom')
    bottom.set(qn('w:val'), 'nil')
    borders.append(bottom)
    title_ppr.append(borders)
    doc.add_paragraph(f'Module {number} {title}', 'Title')
    doc.add_paragraph(introduction)
    doc.add_heading('Starting point and learning goals', 1)
    doc.add_paragraph(f'Start from the completed Module {number-1} branch. Create a new feature branch before editing. Build the Windows target at each checkpoint.')
    doc.add_paragraph('Learning goals: ' + '; '.join(s[1] for s in steps) + '.')
    for i, (heading, goal, work, check) in enumerate(steps, 1):
        doc.add_heading(f'Step {i} {heading}', 1)
        doc.add_paragraph('Concept. ' + goal)
        doc.add_paragraph('Do. ' + work)
        doc.add_paragraph('Check. ' + check)
    doc.add_heading('Full source checkpoints', 1)
    doc.add_paragraph('The files below show the completed module. Read them after following the small steps above, or use them to compare your result.')
    for label, path in files:
        doc.add_heading(label, 2)
        if not path.exists():
            doc.add_paragraph('This file is supplied by the previous module.')
            continue
        for line in path.read_text(encoding='utf-8').splitlines():
            p = doc.add_paragraph(style='Normal')
            p.paragraph_format.space_after = Pt(0)
            p.paragraph_format.left_indent = Inches(.15)
            run = p.add_run(line or ' ')
            run.font.name = 'Consolas'
            run.font.size = Pt(8)
    doc.add_heading('Further reading', 1)
    for ref in references:
        doc.add_paragraph(ref)
    output = folder / f'Module_{number}_{title.replace(" ", "_")}_Tutorial.docx'
    doc.save(output)
    print(output)
    return output

if __name__ == '__main__':
    import sys
    number = int(sys.argv[1])
    source = ROOT / 'ProjectCostEstimator'
    if number == 8:
        build(8, 'MVVM Toolkit Refactor',
            'Refactor the working estimator to use generated observable properties and commands. The app keeps the same behavior while the repeated notification and command setup code disappears. The validation rules remain explicit so you can distinguish framework plumbing from application decisions.',
            [
                ('Add the package', 'Identify which code the toolkit can generate', 'Add CommunityToolkit.Mvvm version 8.4.2 to the project. Restore and build before changing any view model.', 'The existing app still builds.'),
                ('Generate editor properties', 'Use ObservableObject and ObservableProperty', 'Make ItemEditorViewModel partial and inherit ObservableObject. Convert one input field to an observable partial property and build. Convert the remaining inputs and output text properties. The generated property names must match the existing XAML bindings.', 'Typing still updates bound values and the total preview.'),
                ('Keep the editor reactions', 'Use generated change hooks for behavior', 'Implement OnTitleChanged and OnUnitsChanged to update validation. Implement the cost and quantity hooks to update the total and validation. Add NotifyCanExecuteChangedFor to fields affecting Submit.', 'Submit availability and validation respond as each field changes.'),
                ('Generate editor commands', 'Create commands from methods', 'Mark Submit with RelayCommand and CanExecute; mark Cancel with RelayCommand. Remove manual Command construction and ChangeCanExecute calls. Keep the Submitted and CancelRequested events because the page handles navigation.', 'Add, Edit, Cancel, invalid input, and overflow behave as before.'),
                ('Generate BOM commands', 'Apply the same pattern to collection actions', 'Make BillOfMaterialsViewModel partial and inherit ObservableObject. Mark Add, Edit, and Delete methods with RelayCommand. Keep service calls and page requests.', 'Add, Edit, and Delete still update the list.'),
                ('Review generated code', 'Explain what was removed and what remains', 'Compare with Module 7. Point to the generated command names used by XAML. Explain why the validation method is still handwritten.', 'Windows build has zero warnings and errors; the complete workflow works.'),
            ],
            [('ProjectCostEstimator.csproj', source/'ProjectCostEstimator.csproj'), ('ViewModels/ItemEditorViewModel.cs', source/'ViewModels'/'ItemEditorViewModel.cs'), ('ViewModels/BillOfMaterialsViewModel.cs', source/'ViewModels'/'BillOfMaterialsViewModel.cs')],
            ['https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/observableproperty', 'https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/relaycommand'])
