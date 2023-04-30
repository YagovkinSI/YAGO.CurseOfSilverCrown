import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { AppDispatch } from '.';
import { IRequestType, RequestParams, requestHelper } from '../helpers/RequestHelper';
import IDomainPublic from '../apiModels/domainPublic';

export interface ActiveDomainState {
    activeDomain: IDomainPublic | undefined,
    isLoading: boolean,
    error: string
}

export const defaultActiveDomainState: ActiveDomainState = {
    activeDomain: undefined,
    isLoading: false,
    error: ''
}

const loadData = requestHelper.createThunk('activeDomain')

export const activeDomainSlice = createSlice({
    name: 'activeDomain',
    initialState: defaultActiveDomainState,
    reducers: {},
    extraReducers: {
        [loadData.fulfilled.type]: (state, action: PayloadAction<IDomainPublic>) => {
            state.activeDomain = action.payload,
                state.isLoading = false
        },
        [loadData.pending.type]: (state) => {
            state.activeDomain = undefined,
                state.isLoading = true
        },
        [loadData.rejected.type]: (state, action: PayloadAction<string>) => {
            state.activeDomain = undefined,
                state.error = action.payload,
                state.isLoading = false
        }
    }
});

const getDomainPublic = async (dispatch: AppDispatch, domainId: number) => {
    const requestParams: RequestParams = {
        path: 'apiDomain/getDomainPublic',
        type: IRequestType.get,
        data: { params: { domainId } }
    }
    dispatch(loadData(requestParams));
}

export const activeDomainActionCreators = { getDomainPublic };
