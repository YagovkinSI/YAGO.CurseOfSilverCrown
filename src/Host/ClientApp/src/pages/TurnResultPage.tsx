import React, { useEffect } from "react";
import Page from "../widgets/Page";
import ResultSlideRenderer from "../entities/events/ResultSlideRenderer";
import { useNavigate } from "react-router-dom";
import { useUseActionMutation } from "../entities/gameActions/gameActions.api";


const TurnResultPage: React.FC = () => {
    const navigate = useNavigate();

    const [performAction, actionResult] = useUseActionMutation();

    useEffect(() => {
        const fetchResult = async () => {
            const result = await performAction({ type: 'endTurn' }).unwrap();
            if (!result.data) {
                navigate('/me/colony');
            }
        };
        fetchResult();
    }, [performAction, navigate]);

    const isLoading = actionResult.isLoading;
    const error = actionResult.error;

    const eventResultSlide = actionResult.data?.data;

    const renderContent = () => {
        if (!eventResultSlide) return null;
        return <ResultSlideRenderer 
            eventResult={eventResultSlide} 
            onClose={() => navigate('/me/colony')}
            />;
    };

    return (
        <Page backgroundImage="captain_hall" darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
}

export default TurnResultPage;
