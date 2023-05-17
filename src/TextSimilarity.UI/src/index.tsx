import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from 'react-redux';
import { setupStore } from './common/store';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const store = setupStore();

root.render(
  <Provider store = {store}>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </Provider>
);

