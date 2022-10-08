// @ts-ignore
import { Request, Response } from 'express';

export default {
  'POST /api/login/captcha': (req: Request, res: Response) => {
    res.status(200).send({ code: 65, status: 'default' });
  },
};
