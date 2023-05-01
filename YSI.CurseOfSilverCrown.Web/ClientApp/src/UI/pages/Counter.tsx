import * as React from 'react';
import { useAppDispatch, useAppSelector } from '../../store';
import { counterActionCreators } from '../../store/Counter';

const Counter :  React.FC = () => {
    const state = useAppSelector(state => state.counterReducer);
    const dispatch = useAppDispatch();
    
    return (
        <React.Fragment>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p aria-live="polite">Current count: <strong>{state?.count ?? 0}</strong></p>

            <button type="button"
                className="btn btn-primary btn-lg"
                onClick={() => counterActionCreators.increment(dispatch)}>
                Increment
            </button>
        </React.Fragment>
    );
}

export default Counter;
