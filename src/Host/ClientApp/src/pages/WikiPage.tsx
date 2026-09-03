import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import Page from '../widgets/Page';
import SlideRenderer from '../widgets/SlideRenderer';
import type { Slide } from '../entities/events/colonyEvent.types';
import type { WikiArticle } from '../entities/wiki/wiki.types';
import { useGetWikiArticleQuery } from '../entities/wiki/wiki.api';

const WikiPage: React.FC = () => {
    const { code } = useParams();
    const navigate = useNavigate();

    const articleResult = useGetWikiArticleQuery(code ?? '');
    const article = articleResult.data;

    useEffect(() => {
        const notFound = articleResult.isSuccess && article == undefined;
        if (code == undefined || notFound) {
            navigate('/wiki', { replace: true });
        }
    }, [code, article, articleResult.isSuccess, navigate]);

    const buildSlide = (wikiArticle: WikiArticle): Slide => ({
        id: wikiArticle.code,
        title: wikiArticle.name,
        imageName: wikiArticle.image,
        text: wikiArticle.text,
        visibleEffects: [],
        requirements: [],
        buttons: [],
    });

    const renderContent = () => {
        if (article == undefined)
            return null;
        return (
            <SlideRenderer
                slide={buildSlide(article)}
                header={{ leftButton: { icon: ArrowLeft, onClick: () => navigate('/wiki'), label: 'Назад' } }}
                resetScrollTrigger={article.code}
            />
        );
    };

    return (
        <Page backgroundImage='space' darkenBackground isLoading={articleResult.isLoading} error={articleResult.error}>
            {renderContent()}
        </Page>
    );
};

export default WikiPage;
