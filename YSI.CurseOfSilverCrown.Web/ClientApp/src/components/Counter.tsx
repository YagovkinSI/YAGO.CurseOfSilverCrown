import * as React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AppDispatch } from '..';
import { ApplicationState } from '../store';
import * as CounterStore from '../store/Counter';

const Counter :  React.FC = () => {
    const dispatch = useDispatch<AppDispatch>(); 
    const appState = useSelector(state => state as ApplicationState);  
    const state = appState.counter;
    
    return (
        <React.Fragment>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p aria-live="polite">Current count: <strong>{state?.count ?? 0}</strong></p>

            <button type="button"
                className="btn btn-primary btn-lg"
                onClick={() => dispatch(CounterStore.actionCreators.increment())}>
                Increment
            </button>
        </React.Fragment>
    );
}

export default Counter;
