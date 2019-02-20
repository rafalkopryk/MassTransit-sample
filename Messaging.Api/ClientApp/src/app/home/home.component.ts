import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-home',
  template: `
<h1>Hello!</h1>
<p>Fill in the form below to send a message to the queue</p>

<form #f="ngForm" (ngSubmit)="submitMessage(f)" novalidate>

  <div class="form-group">

    <label for="who">Who</label>
    <input type="text"  class="form-control" id="who" name="Who" ngModel required #first="ngModel">

    <label for="who">Who</label>

    <input type="text"  class="form-control" id="who"  name="Content" ngModel required>
  </div>

  <button class="btn form-control btn-success">Send</button>
    
</form>

<p>{{message}}</p>
`,
})
export class HomeComponent {

  private url: string;
  private message: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
  }

  submitMessage(f: NgForm) {
    let message = new Message(f.value.Who, f.value.Content);
    this.http.post<Message>(this.url + 'api/Massage', message)
      .subscribe(() => this.message = "Message was sent ");
  }
}

class Message {
  public Who: string;
  public Content: string;

  constructor(who: string, content: string) {
    this.Who = who;
    this.Content = content;
  }
}
