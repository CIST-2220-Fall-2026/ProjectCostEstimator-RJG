import fs from 'node:fs/promises';
import { readFileSync } from 'node:fs';
import path from 'node:path';
import { pathToFileURL } from 'node:url';
import { FileBlob, PresentationFile } from 'file:///C:/Users/Rob011235/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/@oai/artifact-tool/dist/artifact_tool.mjs';

const ROOT = 'C:/repos/CIST-2220-Fall-2026/ProjectCostEstimator';
const CURRICULUM = 'C:/Users/Rob011235/Dropbox/CNM/_Curriculum/CIST 2220 C# .NET I/MAUI';
const TEMPLATE = 'C:/Users/Rob011235/Dropbox/CNM/_Curriculum/Templates/PresentationTemplate.pptx';
const SKILL = 'C:/Users/Rob011235/.codex/plugins/cache/openai-primary-runtime/presentations/26.1011.11113/skills/presentations';
const PYTHON = 'C:/Users/Rob011235/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe';
process.env.RUNTIME_NODE_MODULES='C:/Users/Rob011235/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules';
const BUILD = path.join(ROOT, '.pptx_build');
const OUTPUT = path.join(ROOT, 'Presentations');

const courses = [
  {n:1, title:'Build the IoT Estimator', overview:['Change the MAUI template for the estimator.','Add a model and an item editor page.','Validate text and numbers before calculation.'], concepts:[
    {s:1,t:'MAUI project files',pre:['XAML defines the visible page.','C# code handles page behavior.','Resources holds images and app artwork.'],post:['Find MainPage, AppShell, and Resources.','Explain the job of each file.'],img:'01-project-folders.png'},
    {s:5,t:'Image resources',pre:['Export the logo as a transparent PNG.','Keep the app icon square.'],post:['The transparent logo is ready for the app.','The Home page can show it in Step 6.'],img:'02-inkscape-logo-export.png'},
    {s:12,t:'MaterialItem model',pre:['A model stores one material item.','TotalCost uses cost and quantity.'],post:['Create one item with the required values.','Check its calculated total.'],img:'06-material-item-model.png'},
    {s:16,t:'Editor page layout',pre:['A vertical layout stacks the inputs.','A ScrollView keeps the form usable.'],post:['The Item Editor shows every input.','The Submit button appears below the form.'],img:'10-item-editor-empty.png'},
    {s:20,t:'Shell navigation',pre:['Shell lists the app destinations.','A route opens the editor page.'],post:['Open the editor through Shell.','Use Back to return to Home.'],img:'09-shell-navigation.png'},
    {s:21,t:'Input validation',pre:['Check required text before calculation.','Use TryParse for numeric input.'],post:['Invalid input shows an alert.','Valid input produces a total.'],img:'14-invalid-number-alert.png'}]},
  {n:2,title:'Bill of Materials',overview:['Return items from the editor.','Add, edit, and delete BOM rows.','Add Help and Shell destinations.'],concepts:[
    {s:2,t:'Return an item',pre:['The editor returns a MaterialItem.','Cancel returns without a new item.'],post:['The page can hold a submitted item.','A new editor starts with empty fields.'],img:'04-new-item-editor.png'},
    {s:6,t:'CollectionView rows',pre:['CollectionView repeats a row template.','BindingContext supplies each row item.'],post:['The BOM shows each sample item.','Each row shows its calculated cost.'],img:'03-bill-of-materials.png'},
    {s:7,t:'Add and refresh',pre:['Add changes the list in memory.','This module refreshes ItemsSource by hand.'],post:['The new row appears once.','Its total matches the editor result.'],img:'05-added-item.png'},
    {s:8,t:'Edit an existing row',pre:['Pass the selected item to the editor.','Replace the original only after Submit.'],post:['Edit starts with the row values.','Submit changes the existing row.'],img:'06-edit-item-prefilled.png'},
    {s:9,t:'Confirm deletion',pre:['Ask before you remove a row.','No keeps the list as it is.'],post:['Yes removes the selected row.','No keeps the selected row.'],img:'08-delete-confirmation.png'},
    {s:11,t:'Shell destinations',pre:['Home, BOM, and Help are top-level pages.','The editor is a temporary page.'],post:['The Shell menu opens each page.','Back returns from the editor.'],img:'02-shell-menu.png'}]},
  {n:3,title:'Item Editor Binding',overview:['Move form values into a view model.','Bind the Entries to those values.','Show a live cost total.'],concepts:[
    {s:2,t:'BindingContext',pre:['BindingContext names the data source.','A two-way binding moves an Entry value both ways.'],post:['Title appears in the bound property.','The page still looks the same.'],img:'02-bound-editor-empty.png'},
    {s:3,t:'Property notification',pre:['A changed C# value must notify the view.','INotifyPropertyChanged sends that notice.'],post:['The view reflects a changed property.','Typing still changes the view-model value.'],img:'03-live-total-preview.png'},
    {s:5,t:'Edit prefill',pre:['Copy the selected item into the view model.','Bindings put those values in the Entries.'],post:['Edit opens with the row values.','Cancel leaves the original item.'],img:'07-edit-prefilled-preview.png'},
    {s:6,t:'Live total',pre:['Calculate after cost or quantity changes.','Notify the TotalText binding.'],post:['The total changes while the user types.','Invalid numbers show a helpful message.'],img:'08-edit-live-preview.png'},
    {s:8,t:'Typed bindings',pre:['x:DataType checks binding names at build time.','A wrong property name becomes a build issue.'],post:['The page builds with typed bindings.','All five Entries still bind.'],img:'06-bound-submit-adds-item.png'}]},
  {n:4,title:'Commands and Validation',overview:['Replace button click handlers with commands.','Keep form validation in the view model.','Disable Submit when values are invalid.'],concepts:[
    {s:1,t:'Submit command',pre:['A command represents an action in the view model.','XAML binds the button to that command.'],post:['Submit still creates an item.','The button has no Clicked handler.'],img:'05-submit-adds-item.png'},
    {s:2,t:'Validation rules',pre:['Keep validation beside the form values.','Create an item only when all values pass.'],post:['Invalid values do not create an item.','Valid values produce the expected total.'],img:'02-invalid-number-submit-disabled.png'},
    {s:3,t:'Validation message',pre:['Bind a message under the form.','Tell the user which value needs work.'],post:['A bad value shows a specific message.','A valid form shows its total.'],img:'04-overflow-submit-disabled.png'},
    {s:4,t:'CanExecute',pre:['CanExecute controls whether a command can run.','Notify the command when input changes.'],post:['Submit is disabled for invalid data.','Submit enables when the form is valid.'],img:'03-valid-form-submit-enabled.png'},
    {s:5,t:'Cancel command',pre:['Cancel is a command with no validation rule.','The page still handles navigation.'],post:['Cancel closes the editor.','The original item stays unchanged.'],img:'08-cancel-keeps-original.png'}]},
  {n:5,title:'BOM View Model',overview:['Move list actions into a view model.','Pass the selected row to Edit and Delete.','Keep the current manual refresh.'],concepts:[
    {s:1,t:'List ownership',pre:['The BOM view model owns the item list.','The page binds to its Items property.'],post:['The same rows still appear.','The page no longer stores the list.'],img:'01-bom-starting-list.png'},
    {s:2,t:'Add command',pre:['The Add button binds to a command.','The view model requests the editor.'],post:['Add opens a blank editor.','Submit adds the returned item.'],img:'03-add-command-refreshes-list.png'},
    {s:4,t:'CommandParameter',pre:['Each row has its own MaterialItem.','CommandParameter passes that item to Edit.'],post:['Edit opens the selected row.','Its values appear in the form.'],img:'04-edit-receives-row-item.png'},
    {s:5,t:'Delete command',pre:['Delete also receives the row item.','The page still asks for confirmation.'],post:['No keeps the row.','Yes removes the selected row.'],img:'08-delete-confirmation.png'}]},
  {n:6,title:'ObservableCollection',overview:['Replace manual list refresh.','Let the collection report changes.','Check Add, Edit, and Delete.'],concepts:[
    {s:1,t:'Manual refresh',pre:['List changes do not refresh this List by themselves.','The page resets ItemsSource after each change.'],post:['Find each manual refresh call.','Explain why the page needs it.'],img:'01-starting-bom.png'},
    {s:2,t:'Collection changes',pre:['ObservableCollection reports Add, Replace, and Remove.','CollectionView listens to these reports.'],post:['A new row appears without a reset.','The page still binds to Items.'],img:'03-added-without-manual-refresh.png'},
    {s:3,t:'Remove the bridge',pre:['The collection now reports list changes.','The old page refresh code has no job.'],post:['Remove the reset calls.','Add still updates the screen.']},
    {s:5,t:'Replace and remove',pre:['Edit replaces one item in the collection.','Delete removes one item.'],post:['Edit changes one row.','Delete removes one row without a reset.'],img:'06-replaced-without-manual-refresh.png'}]},
  {n:7,title:'Services and DI',overview:['Move BOM data into a service.','Register the service and view models.','Inject dependencies through constructors.'],concepts:[
    {s:1,t:'Data ownership',pre:['The view model currently holds the list.','A service can own shared BOM data.'],post:['Name the owner of Items.','Keep commands in the view model.'],img:'01-injected-bom.png'},
    {s:2,t:'Service contract',pre:['IBomService states the available operations.','The view model depends on the contract.'],post:['The interface has Items and change methods.','It has no page or dialog code.'],img:'03-service-add.png'},
    {s:3,t:'In-memory service',pre:['InMemoryBomService implements the contract.','It keeps the BOM for the current app run.'],post:['Add and Replace use the service.','CollectionView shows the changed data.'],img:'05-service-replace.png'},
    {s:4,t:'Dependency injection',pre:['Register each service in MauiProgram.','The container passes dependencies to constructors.'],post:['MAUI creates the page and view model.','The BOM workflow still works.'],img:'06-delete-confirmation.png'}]},
  {n:8,title:'MVVM Toolkit',overview:['Add CommunityToolkit.Mvvm.','Generate observable properties and commands.','Keep validation rules explicit.'],concepts:[
    {s:1,t:'Generated plumbing',pre:['The toolkit generates repeat notification code.','It does not choose validation rules.'],post:['The project builds with the package.','App behavior stays the same.'],img:'module-8-concept.png'},
    {s:2,t:'ObservableProperty',pre:['Mark state for generated properties.','Keep binding names stable.'],post:['Typing still updates the form.','The live total still changes.'],img:'editor-toolkit.png'},
    {s:3,t:'Change hooks',pre:['A generated property can call a change hook.','Use hooks to update validation and total.'],post:['A changed input updates the message.','Submit state follows the input.']},
    {s:4,t:'RelayCommand',pre:['Mark an action with RelayCommand.','The toolkit generates the command property.'],post:['Submit and Cancel still work.','The BOM commands still change rows.'],img:'bom-toolkit.png'}]},
  {n:9,title:'UI Boundaries',overview:['Inject the editor view model.','Move navigation and alerts behind an interface.','Test BOM decisions without a page.'],concepts:[
    {s:1,t:'Editor injection',pre:['The container creates the editor view model.','The page binds to that injected instance.'],post:['Add starts with an empty editor.','Edit starts with selected values.'],img:'injected-editor.png'},
    {s:2,t:'Interaction results',pre:['The interface returns an item after Submit.','Null means Cancel or Back.'],post:['Cancel does not change the BOM.','Submit returns the edited item.'],img:'module-9-concept.png'},
    {s:3,t:'MAUI adapter',pre:['The adapter opens pages and alerts.','The view model uses its result.'],post:['Delete shows a confirmation alert.','No leaves the row in place.'],img:'confirmation-service.png'},
    {s:4,t:'BOM decisions',pre:['The BOM view model decides Add, Edit, and Delete.','The adapter performs UI work.'],post:['The page has no BOM decision code.','All user actions still work.']}]},
  {n:10,title:'JSON Persistence',overview:['Save BOM items in a JSON file.','Reload them after restart.','Test storage and view-model decisions.'],concepts:[
    {s:1,t:'Async storage',pre:['File work can take time.','Task-returning methods keep the UI responsive.'],post:['All callers await the service.','The app still builds.'],img:'module-10-concept.png'},
    {s:2,t:'JSON service',pre:['JsonBomService writes the item list.','A new service instance loads it.'],post:['A service instance reloads saved items.','The JSON data has the item values.']},
    {s:3,t:'Failed save',pre:['A failed write must not leave a false row.','Restore the collection after an error.'],post:['The visible list matches saved data.','The page shows an error message.']},
    {s:4,t:'Page lifecycle',pre:['Load when the BOM page appears.','Use FileSystem.AppDataDirectory for the path.'],post:['The saved row appears on return.','An edit survives restart.'],img:'reloaded-item.png'},
    {s:5,t:'Automated tests',pre:['Test storage with a temporary file.','Test decisions with a fake interaction service.'],post:['Save and reload tests pass.','Cancel and Delete No keep the list.']}]},
  {n:11,title:'MVVM Redo',overview:['Create a new MAUI project.','Build the estimator with MVVM from the start.','Compare your result with the reference.'],concepts:[
    {s:1,t:'New MAUI project',pre:['Create a .NET MAUI App in Visual Studio.','Name it ProjectCostEstimator.'],post:['The template app runs on Windows.','Keep the reference closed for now.']},
    {s:2,t:'Responsibilities',pre:['Views show controls and handle platform UI.','View models own form and list decisions.','Services own storage.'],post:['Draw the three layers.','Explain where navigation belongs.'],img:'module-11-concept.png'},
    {s:3,t:'Model and service',pre:['MaterialItem holds item data.','IBomService states storage operations.'],post:['A sample item has the right total.','The in-memory service changes the list.']},
    {s:5,t:'Editor view model',pre:['Generated properties hold form state.','Validation controls Submit.'],post:['The page binds to the view model.','Valid values enable Submit.'],img:'reference-editor.png'},
    {s:7,t:'BOM view model',pre:['Commands own Add, Edit, and Delete decisions.','An interface supplies UI results.'],post:['The BOM shows the changed rows.','Cancel keeps the original row.'],img:'reference-bom.png'},
    {s:9,t:'App composition',pre:['Register services, view models, and pages.','Shell opens the app destinations.'],post:['Home, BOM, and Help open.','The editor opens for Add and Edit.'],img:'reference-home.png'},
    {s:10,t:'JSON persistence',pre:['Swap in JsonBomService.','Load items when the BOM page appears.'],post:['Items survive restart.','The same BOM workflow still works.']},
    {s:11,t:'Test the redo',pre:['Test save and reload.','Test decisions with a fake UI service.'],post:['Automated tests pass.','Add, Edit, Cancel, and Delete work.']}]},
];

function imagePath(n, filename) {
  if (n <= 7) return path.join(CURRICULUM, `Module ${n}`, `Module_${n}_Screen_Captures`, filename);
  return path.join(ROOT, 'Tutorials', `Module ${n}`, filename);
}

function fillBullets(slide, bullets, image) {
  const body = slide.placeholders.getItem('content placeholder 2');
  body.text = bullets.join('\n');
  if (image) {
    slide.images.add({blob:new Uint8Array(readFileSync(image)),contentType:'image/png', alt:'Screen capture or concept diagram for the tutorial checkpoint',fit:'contain',position:{left:280,top:335,width:720,height:320}});
  }
}

function contentSlide(p, title, bullets, notes, image) {
  const slide=p.slides.add({layoutId:p.layouts.items[2].id});
  slide.placeholders.addFromLayout();
  slide.placeholders.getItem('title').text=title;
  fillBullets(slide,bullets,image);
  slide.speakerNotes.text=notes;
  return slide;
}

const { finalizePresentation } = await import(pathToFileURL(path.join(SKILL,'container_tools','artifact_tool_utils.mjs')).href);
await fs.mkdir(BUILD,{recursive:true});
await fs.mkdir(OUTPUT,{recursive:true});

for (const course of courses) {
  if (process.env.MODULE && !process.env.MODULE.split(',').map(Number).includes(course.n)) continue;
  const p=await PresentationFile.importPptx(await FileBlob.load(TEMPLATE));
  const cover=p.slides.items[0];
  cover.placeholders.getItem('title').text=`Module ${course.n} ${course.title}`;
  cover.placeholders.getItem('subtitle').text='IoT Project Cost Estimator';
  cover.speakerNotes.text=`Introduce Module ${course.n}. Open the matching Word tutorial after the exercise overview.`;
  contentSlide(p,'Exercise overview',course.overview,`Explain the work for Module ${course.n}. Show the expected app result before students start. Source: Module ${course.n} Word tutorial.`);
  for (const c of course.concepts) {
    contentSlide(p,`Step ${c.s} ${c.t}`,c.pre,`Before tutorial Step ${c.s}: explain these concepts. Ask students to predict what the change will do. Then open Step ${c.s} in the Word tutorial. Source: Module ${course.n} Word tutorial.`);
    const picture=c.img ? imagePath(course.n,c.img) : undefined;
    if (picture && !(await fs.stat(picture).catch(()=>null))) throw new Error(`Missing image ${picture}`);
    contentSlide(p,`Step ${c.s} Check`,c.post,`After tutorial Step ${c.s}: run the app or inspect the code. Ask students to explain the observed result. Source: Module ${course.n} Word tutorial.`,picture);
  }
  contentSlide(p,'Module review',['Show the completed app workflow.','Explain which responsibility changed.','Check the result before the next module.'],`Use the final tutorial checklist. Ask students to explain the module concept without reading code. Source: Module ${course.n} Word tutorial.`);
  const stem=`Module_${course.n}_${course.title.replaceAll(' ','_')}_Presentation_${process.env.REVISION || 'v2'}`;
  const candidate=path.join(BUILD,`${stem}.draft.pptx`);
  const final=path.join(OUTPUT,`${stem}.pptx`);
  const canonical=path.join(OUTPUT,`Module_${course.n}_${course.title.replaceAll(' ','_')}_Presentation.pptx`);
  await (await PresentationFile.exportPptx(p)).save(candidate);
  const result=await finalizePresentation({workspaceDir:ROOT,candidatePath:candidate,finalPath:final,pythonExecutable:PYTHON,integrityValidatorPath:path.join(SKILL,'container_tools','inspect_presentation_package_integrity.py'),layoutValidatorPath:path.join(SKILL,'container_tools','inspect_presentation_layout_geometry.py'),layoutArgs:['--expected-slide-size-emu','12192000,6858000','--validate-heading-fit'],sourceTemplatePath:TEMPLATE,verifyArtifactToolImport:true,receiptPath:path.join(BUILD,`${stem}.validation.json`)});
  const target=path.join(CURRICULUM,`Module ${course.n}`,path.basename(canonical));
  await fs.copyFile(final,canonical);
  await fs.copyFile(final,target);
  await fs.unlink(final);
  console.log(course.n,p.slides.items.length,canonical,target,result?.status ?? 'finalized');
}
