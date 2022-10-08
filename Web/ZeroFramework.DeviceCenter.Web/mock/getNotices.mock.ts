// @ts-ignore
import { Request, Response } from 'express';

export default {
  'GET /api/notices': (req: Request, res: Response) => {
    res.status(200).send({
      data: [
        {
          id: 'D9cb6ecf-CDc9-D2bE-9D41-dAB6efcb1Ddc',
          extra: '6l&(XM',
          key: 13,
          read: true,
          avatar: 'https://gw.alipayobjects.com/zos/rmsportal/udxAbMEhpwthVVcjLXik.png',
          title: '小种指包着两数称近路些步称复志须圆。',
          status: 'success',
          datetime: '2015-05-07',
          description: '和律能自其志记常难斯进展温实权地。',
          type: 'notification',
        },
        {
          id: 'e3c3e932-E3dA-17e6-AB2B-aFe3ACe0AF53',
          extra: '@3c4$',
          key: 14,
          read: false,
          avatar: 'https://gw.alipayobjects.com/zos/rmsportal/OKJXDXrmkNshAMvwtvhu.png',
          title: '今元速比今易米出却地无热专。',
          status: 'processing',
          datetime: '1995-12-04',
          description: '身去设可强周根共程支算器步是千。',
          type: 'notification',
        },
        {
          id: 'EbdAd2F1-F019-3cEC-DBf5-e774bEaf91E9',
          extra: 'Lunxd',
          key: 15,
          read: false,
          avatar: 'https://gw.alipayobjects.com/zos/rmsportal/udxAbMEhpwthVVcjLXik.png',
          title: '如级矿教采名我商老段可间圆明等。',
          status: 'warning',
          datetime: '1989-07-20',
          description: '志前装流理设受属太千队角平。',
          type: 'notification',
        },
        {
          id: '54F57AEF-1948-aF9e-D7A5-339326cf3Da8',
          extra: 'Brz',
          key: 16,
          read: true,
          avatar: '',
          title: '建连么各运解共老那没九包律提文水。',
          status: 'processing',
          datetime: '1985-03-16',
          description: '往料们带产内每经种内史力对保。',
          type: 'notification',
        },
        {
          id: '351CF59d-ee2E-6037-bBac-bd33bbbBA580',
          extra: 'HN94fM',
          key: 17,
          read: false,
          avatar: 'https://avatars0.githubusercontent.com/u/507615?s=40&v=4',
          title: '任毛如根八情基内南厂者龙元达干次。',
          status: 'warning',
          datetime: '2010-09-28',
          description: '家参路五式准月术进回技应先。',
          type: 'notification',
        },
      ],
      total: 84,
      success: false,
    });
  },
};
