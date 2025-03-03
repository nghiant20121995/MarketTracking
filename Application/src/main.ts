import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './component/app/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
